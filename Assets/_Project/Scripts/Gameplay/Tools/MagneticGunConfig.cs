using PhysicsHeist.Core.Config;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [CreateAssetMenu(menuName = "PhysicsHeist/Tools/Magnetic Gun Config", fileName = "MagneticGunConfig")]
    public sealed class MagneticGunConfig : ConfigAsset
    {
        [Header("Targeting")]
        [SerializeField, Min(0.1f)] private float range = 25f;
        [SerializeField] private LayerMask targetMask = ~0;

        [Header("Forces (continuous, newtons)")]
        [Tooltip("Continuous force (N) applied to the hit rigidbody toward the gun while Primary Fire is held.")]
        [SerializeField, Min(0f)] private float pullForce = 60f;
        [Tooltip("Continuous force (N) applied to the hit rigidbody away from the gun while Secondary Fire is held.")]
        [SerializeField, Min(0f)] private float pushForce = 120f;
        [Tooltip("Ignore the target when closer than this (prevents jitter at the muzzle).")]
        [SerializeField, Min(0f)] private float minForceDistance = 0.5f;

        public float Range => range;
        public LayerMask TargetMask => targetMask;
        public float PullForce => pullForce;
        public float PushForce => pushForce;
        public float MinForceDistance => minForceDistance;
    }
}
