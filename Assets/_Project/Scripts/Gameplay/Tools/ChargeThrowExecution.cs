using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// Spawns an <see cref="ExplosiveCharge"/> as a physics projectile in the
    /// aim direction. The charge's own <c>OnCollisionEnter</c> handles sticking
    /// to whatever it first touches — no raycast, no surface requirement on
    /// the thrower side. Pair with <see cref="DirectTargetingStrategy"/>.
    ///
    /// We disable collision between the spawned charge and all of the owner's
    /// colliders on spawn so the thrown charge doesn't immediately collide with
    /// (and stick to) the player body. The ignore persists until the charge is
    /// destroyed; once kinematic+parented post-stick it's irrelevant.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ChargeThrowExecution : MonoBehaviour, IExecutionStrategy
    {
        [SerializeField] private ExplosiveChargeConfig config;
        [SerializeField] private ExplosiveCharge chargePrefab;
        [Tooltip("Meters in front of the aim origin where the charge spawns.")]
        [SerializeField, Min(0f)] private float spawnOffset = 0.4f;
        [Tooltip("Forward throw speed in m/s.")]
        [SerializeField, Min(0f)] private float throwSpeed = 15f;
        [Tooltip("Extra upward velocity (m/s) added for a small arc. 0 = flat throw.")]
        [SerializeField, Min(0f)] private float upwardBoost = 1.5f;

        public void Execute(in ToolContext context, in ToolTarget target)
        {
            if (config == null || chargePrefab == null) return;
            if (ExplosiveCharge.LiveCharges.Count >= config.MaxActiveCharges) return;

            var position = context.Origin + context.Direction * spawnOffset;
            var rotation = context.Direction.sqrMagnitude > 1e-6f
                ? Quaternion.LookRotation(context.Direction)
                : Quaternion.identity;

            var instance = Object.Instantiate(chargePrefab, position, rotation);
            instance.Initialize(config, context.Owner);

            var rb = instance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = context.Direction * throwSpeed + Vector3.up * upwardBoost;
                rb.angularVelocity = Random.insideUnitSphere * 1.5f;
            }

            if (context.Owner != null)
            {
                var chargeCollider = instance.GetComponent<Collider>();
                if (chargeCollider != null)
                {
                    var ownerCols = context.Owner.GetComponentsInChildren<Collider>();
                    foreach (var oc in ownerCols)
                        if (oc != null && oc != chargeCollider)
                            UnityEngine.Physics.IgnoreCollision(chargeCollider, oc, true);
                }
            }
        }
    }
}
