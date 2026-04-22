using PhysicsHeist.Core.Physics;
using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [DisallowMultipleComponent]
    public sealed class MagneticPullExecution : MonoBehaviour, IExecutionStrategy
    {
        [SerializeField] private MagneticGunConfig config;

        public void Execute(in ToolContext context, in ToolTarget target)
        {
            if (config == null || target.Rigidbody == null) return;

            var toOrigin = context.Origin - target.Rigidbody.worldCenterOfMass;
            var distance = toOrigin.magnitude;
            if (distance < config.MinImpulseDistance) return;

            var impulse = (toOrigin / distance) * config.PullImpulse;

            var receiver = target.Rigidbody.GetComponent<IForceReceiver>();
            if (receiver != null)
                receiver.ApplyForce(impulse, ForceMode.Impulse);
            else
                target.Rigidbody.AddForce(impulse, ForceMode.Impulse);
        }
    }
}
