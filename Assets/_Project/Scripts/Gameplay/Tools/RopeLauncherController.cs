using PhysicsHeist.Core.Input;
using PhysicsHeist.Core.Tools;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// Primary Fire toggles the rope — press once to fire, press again to
    /// release. Secondary Fire (held) winds the rope in, shortening the max
    /// distance so the SpringJoint reels the two endpoints together.
    ///
    /// Mass weighting is automatic via <see cref="SpringJoint"/>:
    ///  • Anchor with no Rigidbody (world geometry) → connectedBody is null,
    ///    so the spring force acts only on the player.
    ///  • Anchor with a Rigidbody → equal-and-opposite forces are applied to
    ///    player and anchor; with F=ma, the lighter body accelerates more,
    ///    so pulling a 10-ton statue barely moves it while a crate comes to
    ///    the player.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RopeLauncherController : MonoBehaviour
    {
        [SerializeField] private RopeLauncherConfig config;
        [SerializeField] private Transform aimOrigin;
        [SerializeField] private Tool shotTool;
        [SerializeField] private RopeShotExecution shotExecution;
        [SerializeField] private GameObject ownerRoot;

        private IInputService _input;

        [Inject]
        private void Construct(IInputService input)
        {
            _input = input;
        }

        private void Update()
        {
            if (_input == null || config == null || aimOrigin == null) return;

            if (_input.PrimaryFirePressedThisFrame)
            {
                if (shotExecution != null && shotExecution.HasActiveRope)
                    shotExecution.ReleaseActive();
                else if (shotTool != null)
                    Fire();
            }

            if (_input.SecondaryFireHeld && shotExecution != null && shotExecution.HasActiveRope)
                shotExecution.WindIn(config.WindSpeed * Time.deltaTime);
        }

        private void Fire()
        {
            var owner = ownerRoot != null ? ownerRoot : gameObject;
            var ctx = new ToolContext(
                aimOrigin.position,
                aimOrigin.forward,
                config.Range,
                owner,
                config.TargetMask);
            shotTool.TryUse(ctx);
        }
    }
}
