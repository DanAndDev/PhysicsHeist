using UnityEngine;

namespace PhysicsHeist.Core.Config
{
    [CreateAssetMenu(menuName = "PhysicsHeist/Config/Game Config", fileName = "GameConfig")]
    public sealed class GameConfig : ConfigAsset
    {
        [Header("World")]
        [SerializeField] private Vector3 gravity = new(0f, -9.81f, 0f);

        [Header("Tool Forces")]
        [SerializeField, Min(0f)] private float globalForceMultiplier = 1f;

        public Vector3 Gravity => gravity;
        public float GlobalForceMultiplier => globalForceMultiplier;

        protected override void Validate()
        {
            if (globalForceMultiplier < 0f) globalForceMultiplier = 0f;
        }
    }
}
