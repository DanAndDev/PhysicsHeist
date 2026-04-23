using PhysicsHeist.Core.Input;
using PhysicsHeist.Core.Tools;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Tools
{
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

            if (_input.PrimaryFirePressedThisFrame && shotTool != null)
                Fire();

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
