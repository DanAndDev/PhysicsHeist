using PhysicsHeist.Core.Physics;
using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// Continuous push: applies <see cref="MagneticGunConfig.PushForce"/> in
    /// newtons along the origin-to-target vector, at the hit point so the
    /// object also gets a plausible torque. See <see cref="MagneticPullExecution"/>
    /// for the framerate note.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MagneticPushExecution : MonoBehaviour, IExecutionStrategy
    {
        [SerializeField] private MagneticGunConfig config;

        public void Execute(in ToolContext context, in ToolTarget target)
        {
            if (config == null || target.Rigidbody == null) return;

            var fromOrigin = target.Rigidbody.worldCenterOfMass - context.Origin;
            var distance = fromOrigin.magnitude;
            if (distance < config.MinForceDistance) return;

            var force = (fromOrigin / distance) * config.PushForce;

            var receiver = target.Rigidbody.GetComponent<IForceReceiver>();
            if (receiver != null)
                receiver.ApplyForceAtPoint(force, target.Point, ForceMode.Force);
            else
                target.Rigidbody.AddForceAtPosition(force, target.Point, ForceMode.Force);
        }
    }
}
