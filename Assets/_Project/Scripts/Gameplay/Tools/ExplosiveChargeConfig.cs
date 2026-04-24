using PhysicsHeist.Core.Config;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [CreateAssetMenu(menuName = "PhysicsHeist/Tools/Explosive Charge Config", fileName = "ExplosiveChargeConfig")]
    public sealed class ExplosiveChargeConfig : ConfigAsset
    {
        [Header("Throw")]
        [Tooltip("Minimum gap between thrown charges (seconds).")]
        [SerializeField, Min(0f)] private float throwCooldown = 0.35f;
        [Tooltip("How many charges can be live at once. Throws are suppressed past this cap.")]
        [SerializeField, Min(1)] private int maxActiveCharges = 3;

        [Header("Fuse")]
        [SerializeField, Min(0f)] private float fuseTime = 3f;
        [SerializeField] private bool autoDetonate = true;

        [Header("Detonation")]
        [SerializeField, Min(0.1f)] private float blastRadius = 5f;
        [SerializeField, Min(0f)] private float blastImpulse = 30f;
        [SerializeField, Min(0f)] private float blastDamage = 75f;
        [SerializeField] private LayerMask blastMask = ~0;

        public float ThrowCooldown => throwCooldown;
        public int MaxActiveCharges => maxActiveCharges;
        public float FuseTime => fuseTime;
        public bool AutoDetonate => autoDetonate;
        public float BlastRadius => blastRadius;
        public float BlastImpulse => blastImpulse;
        public float BlastDamage => blastDamage;
        public LayerMask BlastMask => blastMask;
    }
}
