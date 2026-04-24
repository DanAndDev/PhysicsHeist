using PhysicsHeist.Core.Input;
using PhysicsHeist.Core.Tools;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// Drives pull / push while the corresponding mouse button is HELD,
    /// not just on the initial press. Each frame we re-raycast and re-apply
    /// force via the underlying <see cref="Tool"/> — the Tool cooldown is
    /// expected to be 0 for this controller so presses-per-second doesn't
    /// gate the continuous effect.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MagneticGunController : MonoBehaviour
    {
        [SerializeField] private MagneticGunConfig config;
        [SerializeField] private Transform aimOrigin;
        [SerializeField] private Tool pullTool;
        [SerializeField] private Tool pushTool;
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

            if (_input.PrimaryFireHeld && pullTool != null)
                Fire(pullTool);

            if (_input.SecondaryFireHeld && pushTool != null)
                Fire(pushTool);
        }

        private void Fire(Tool tool)
        {
            var owner = ownerRoot != null ? ownerRoot : gameObject;
            var ctx = new ToolContext(
                aimOrigin.position,
                aimOrigin.forward,
                config.Range,
                owner,
                config.TargetMask);
            tool.TryUse(ctx);
        }
    }
}
