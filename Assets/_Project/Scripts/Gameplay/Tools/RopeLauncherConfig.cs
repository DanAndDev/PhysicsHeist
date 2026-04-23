using PhysicsHeist.Core.Config;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [CreateAssetMenu(menuName = "PhysicsHeist/Tools/Rope Launcher Config", fileName = "RopeLauncherConfig")]
    public sealed class RopeLauncherConfig : ConfigAsset
    {
        [Header("Targeting")]
        [SerializeField, Min(0.1f)] private float range = 30f;
        [SerializeField] private LayerMask targetMask = ~0;
        [SerializeField, Min(0f)] private float shotCooldown = 0.2f;

        [Header("Rope Physics")]
        [SerializeField, Min(0f)] private float spring = 200f;
        [SerializeField, Min(0f)] private float damper = 15f;
        [SerializeField, Min(0.1f)] private float minLength = 1f;
        [SerializeField, Min(0.1f)] private float maxLength = 40f;

        [Header("Winding")]
        [SerializeField, Min(0f)] private float windSpeed = 6f;

        public float Range => range;
        public LayerMask TargetMask => targetMask;
        public float ShotCooldown => shotCooldown;
        public float Spring => spring;
        public float Damper => damper;
        public float MinLength => minLength;
        public float MaxLength => maxLength;
        public float WindSpeed => windSpeed;
    }
}
