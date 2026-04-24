using PhysicsHeist.Core.Physics;
using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// Continuous pull: applies <see cref="MagneticGunConfig.PullForce"/> in
    /// newtons along the target-to-origin vector. Called every frame while
    /// Primary Fire is held — Unity's physics system accumulates ForceMode.Force
    /// contributions added during Update and integrates them in the next
    /// FixedUpdate, so a steady per-Update call produces a steady continuous
    /// force regardless of framerate.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MagneticPullExecution : MonoBehaviour, IExecutionStrategy
    {
        [SerializeField] private MagneticGunConfig config;

        public void Execute(in ToolContext context, in ToolTarget target)
        {
            if (config == null || target.Rigidbody == null) return;

            var toOrigin = context.Origin - target.Rigidbody.worldCenterOfMass;
            var distance = toOrigin.magnitude;
            if (distance < config.MinForceDistance) return;

            var force = (toOrigin / distance) * config.PullForce;

            var receiver = target.Rigidbody.GetComponent<IForceReceiver>();
            if (receiver != null)
                receiver.ApplyForce(force, ForceMode.Force);
            else
                target.Rigidbody.AddForce(force, ForceMode.Force);
        }
    }
}
