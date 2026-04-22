using PhysicsHeist.Core.Input;
using PhysicsHeist.Core.Tools;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Tools
{
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

            if (_input.PrimaryFirePressedThisFrame && pullTool != null)
                Fire(pullTool);

            if (_input.SecondaryFirePressedThisFrame && pushTool != null)
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
