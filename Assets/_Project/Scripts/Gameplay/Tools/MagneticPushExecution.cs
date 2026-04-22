using PhysicsHeist.Core.Physics;
using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [DisallowMultipleComponent]
    public sealed class MagneticPushExecution : MonoBehaviour, IExecutionStrategy
    {
        [SerializeField] private MagneticGunConfig config;

        public void Execute(in ToolContext context, in ToolTarget target)
        {
            if (config == null || target.Rigidbody == null) return;

            var fromOrigin = target.Rigidbody.worldCenterOfMass - context.Origin;
            var distance = fromOrigin.magnitude;
            if (distance < config.MinImpulseDistance) return;

            var impulse = (fromOrigin / distance) * config.PushImpulse;

            var receiver = target.Rigidbody.GetComponent<IForceReceiver>();
            if (receiver != null)
                receiver.ApplyForceAtPoint(impulse, target.Point, ForceMode.Impulse);
            else
                target.Rigidbody.AddForceAtPosition(impulse, target.Point, ForceMode.Impulse);
        }
    }
}
