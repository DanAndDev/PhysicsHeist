using System.Collections.Generic;
using PhysicsHeist.Core.Input;
using PhysicsHeist.Core.Tools;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// Primary Fire throws a physics charge in the aim direction; Secondary
    /// Fire detonates all currently-live charges at once. No raycast / surface
    /// requirement — the charge flies until it hits something and then sticks
    /// via its own collision handler.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ExplosiveChargeController : MonoBehaviour
    {
        [SerializeField] private ExplosiveChargeConfig config;
        [SerializeField] private Transform aimOrigin;
        [SerializeField] private Tool throwTool;
        [SerializeField] private GameObject ownerRoot;

        private readonly List<ExplosiveCharge> _detonateBuffer = new();
        private IInputService _input;

        [Inject]
        private void Construct(IInputService input)
        {
            _input = input;
        }

        private void Update()
        {
            if (_input == null || config == null || aimOrigin == null) return;

            if (_input.PrimaryFirePressedThisFrame && throwTool != null)
                Throw();

            if (_input.SecondaryFirePressedThisFrame)
                DetonateAll();
        }

        private void Throw()
        {
            var owner = ownerRoot != null ? ownerRoot : gameObject;
            // DirectTargetingStrategy ignores Range/LayerMask, but ToolContext
            // still wants non-default values — we pass sentinels.
            var ctx = new ToolContext(
                aimOrigin.position,
                aimOrigin.forward,
                range: 1f,
                owner,
                layerMask: ~0);
            throwTool.TryUse(ctx);
        }

        private void DetonateAll()
        {
            var charges = ExplosiveCharge.LiveCharges;
            if (charges.Count == 0) return;

            _detonateBuffer.Clear();
            foreach (var c in charges)
                if (c != null) _detonateBuffer.Add(c);

            for (var i = 0; i < _detonateBuffer.Count; i++)
                _detonateBuffer[i].Detonate();

            _detonateBuffer.Clear();
        }
    }
}
