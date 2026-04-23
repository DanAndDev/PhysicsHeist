using System;
using System.Collections.Generic;
using PhysicsHeist.Core.Physics;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    public sealed class ExplosiveCharge : MonoBehaviour
    {
        private static readonly HashSet<ExplosiveCharge> LiveSet = new();
        private static readonly Collider[] OverlapBuffer = new Collider[64];
        private static readonly HashSet<Rigidbody> ProcessedBodies = new();
        private static readonly HashSet<IBreakable> ProcessedBreakables = new();

        public static IReadOnlyCollection<ExplosiveCharge> LiveCharges => LiveSet;

        private ExplosiveChargeConfig _config;
        private GameObject _owner;
        private float _detonateAt;
        private bool _stuck;
        private bool _detonated;

        public event Action<ExplosiveCharge> Detonated;

        public void Initialize(ExplosiveChargeConfig config, GameObject owner)
        {
            _config = config;
            _owner = owner;

            _detonateAt = (_config != null && _config.AutoDetonate && _config.FuseTime > 0f)
                ? Time.time + _config.FuseTime
                : float.PositiveInfinity;
        }

        private void OnEnable() => LiveSet.Add(this);
        private void OnDisable() => LiveSet.Remove(this);

        private void Update()
        {
            if (_detonated) return;
            if (Time.time >= _detonateAt) Detonate();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_stuck || _detonated) return;
            if (_owner != null && collision.collider.transform.IsChildOf(_owner.transform)) return;

            _stuck = true;
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            transform.SetParent(collision.collider.transform, worldPositionStays: true);
        }

        public void Detonate()
        {
            if (_detonated) return;
            _detonated = true;

            if (_config != null) ApplyBlast();

            Detonated?.Invoke(this);
            Destroy(gameObject);
        }

        private void ApplyBlast()
        {
            var origin = transform.position;
            var radius = _config.BlastRadius;
            var impulse = _config.BlastImpulse;
            var damage = _config.BlastDamage;
            var mask = _config.BlastMask;

            var count = UnityEngine.Physics.OverlapSphereNonAlloc(
                origin, radius, OverlapBuffer, mask, QueryTriggerInteraction.Ignore);

            ProcessedBodies.Clear();
            ProcessedBreakables.Clear();

            for (var i = 0; i < count; i++)
            {
                var col = OverlapBuffer[i];
                OverlapBuffer[i] = null;
                if (col == null) continue;

                var rb = col.attachedRigidbody;
                if (rb != null && ProcessedBodies.Add(rb))
                {
                    var receiver = rb.GetComponent<IForceReceiver>();
                    if (receiver != null)
                        receiver.ApplyRadialForce(origin, impulse, radius, ForceMode.Impulse);
                    else
                        rb.AddExplosionForce(impulse, origin, radius, 0f, ForceMode.Impulse);
                }

                if (damage > 0f)
                {
                    var breakable = col.GetComponentInParent<IBreakable>();
                    if (breakable != null && !breakable.IsBroken && ProcessedBreakables.Add(breakable))
                    {
                        var info = new BreakageInfo(origin, damage, gameObject);
                        breakable.ApplyDamage(in info);
                    }
                }
            }

            ProcessedBodies.Clear();
            ProcessedBreakables.Clear();
        }
    }
}
