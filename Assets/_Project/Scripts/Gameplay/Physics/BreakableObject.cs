using System;
using PhysicsHeist.Core.Physics;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Physics
{
    public class BreakableObject : PhysicsObject, IBreakable
    {
        [SerializeField, Min(0f)] private float maxStructuralHealth = 100f;
        [SerializeField, Min(0f)] private float impactForceThreshold = 20f;
        [SerializeField, Min(0f)] private float impactDamageMultiplier = 1f;

        private float _health;
        private bool _isBroken;

        public float StructuralHealth => _health;
        public float MaxStructuralHealth => maxStructuralHealth;
        public bool IsBroken => _isBroken;

        public event Action<IBreakable, BreakageInfo> Broken;

        protected override void Awake()
        {
            base.Awake();
            _health = maxStructuralHealth;
        }

        public virtual void ApplyDamage(in BreakageInfo info)
        {
            if (_isBroken) return;

            _health = Mathf.Max(0f, _health - info.Magnitude);
            if (_health <= 0f)
                Break(info);
        }

        protected virtual void Break(in BreakageInfo info)
        {
            _isBroken = true;
            _health = 0f;
            Broken?.Invoke(this, info);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isBroken) return;

            var impactForce = collision.impulse.magnitude / Mathf.Max(Time.fixedDeltaTime, 1e-4f);
            if (impactForce < impactForceThreshold) return;

            var damage = (impactForce - impactForceThreshold) * impactDamageMultiplier;
            var point = collision.contactCount > 0 ? collision.GetContact(0).point : transform.position;
            ApplyDamage(new BreakageInfo(point, damage, collision.gameObject));
        }
    }
}
