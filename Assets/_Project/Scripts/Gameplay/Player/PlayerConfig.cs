using PhysicsHeist.Core.Config;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Player
{
    [CreateAssetMenu(menuName = "PhysicsHeist/Config/Player", fileName = "PlayerConfig")]
    public sealed class PlayerConfig : ConfigAsset
    {
        [Header("Movement")]
        [Min(0f)] public float moveSpeed = 5f;
        [Min(0f)] public float acceleration = 20f;
        [Min(0f)] public float airControlMultiplier = 0.4f;

        [Header("Jump")]
        [Min(0f)] public float jumpImpulse = 5f;

        [Header("Grounding")]
        [Min(0f)] public float groundCheckDistance = 0.3f;
        [Min(0f)] public float groundCheckRadius = 0.3f;
        public LayerMask groundMask = ~0;

        [Header("Look")]
        [Min(0f)] public float mouseSensitivity = 0.12f;
        [Range(-89f, 0f)] public float minPitch = -85f;
        [Range(0f, 89f)] public float maxPitch = 85f;

        protected override void Validate()
        {
            if (maxPitch < minPitch) maxPitch = minPitch;
        }
    }
}
