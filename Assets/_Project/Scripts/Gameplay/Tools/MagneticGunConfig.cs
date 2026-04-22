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

        [Header("Forces")]
        [SerializeField, Min(0f)] private float pullImpulse = 12f;
        [SerializeField, Min(0f)] private float pushImpulse = 20f;
        [SerializeField, Min(0f)] private float minImpulseDistance = 0.5f;

        [Header("Timing")]
        [SerializeField, Min(0f)] private float pullCooldown = 0.1f;
        [SerializeField, Min(0f)] private float pushCooldown = 0.25f;

        public float Range => range;
        public LayerMask TargetMask => targetMask;
        public float PullImpulse => pullImpulse;
        public float PushImpulse => pushImpulse;
        public float MinImpulseDistance => minImpulseDistance;
        public float PullCooldown => pullCooldown;
        public float PushCooldown => pushCooldown;
    }
}
