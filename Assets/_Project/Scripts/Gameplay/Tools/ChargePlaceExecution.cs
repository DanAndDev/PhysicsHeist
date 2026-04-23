using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [DisallowMultipleComponent]
    public sealed class ChargePlaceExecution : MonoBehaviour, IExecutionStrategy
    {
        [SerializeField] private ExplosiveChargeConfig config;
        [SerializeField] private ExplosiveCharge chargePrefab;
        [SerializeField, Min(0f)] private float surfaceOffset = 0.08f;

        public void Execute(in ToolContext context, in ToolTarget target)
        {
            if (config == null || chargePrefab == null) return;
            if (!target.Valid) return;

            if (ExplosiveCharge.LiveCharges.Count >= config.MaxActiveCharges) return;

            var normal = target.Normal.sqrMagnitude > 1e-6f ? target.Normal : Vector3.up;
            var rotation = Quaternion.LookRotation(-normal, Vector3.up);
            var position = target.Point + normal * surfaceOffset;

            var instance = Object.Instantiate(chargePrefab, position, rotation);
            instance.Initialize(config, context.Owner);
        }
    }
}
