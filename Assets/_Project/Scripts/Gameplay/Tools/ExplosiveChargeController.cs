using System.Collections.Generic;
using PhysicsHeist.Core.Input;
using PhysicsHeist.Core.Tools;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Tools
{
    [DisallowMultipleComponent]
    public sealed class ExplosiveChargeController : MonoBehaviour
    {
        [SerializeField] private ExplosiveChargeConfig config;
        [SerializeField] private Transform aimOrigin;
        [SerializeField] private Tool placeTool;
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

            if (_input.PrimaryFirePressedThisFrame && placeTool != null)
                Place();

            if (_input.SecondaryFirePressedThisFrame)
                DetonateAll();
        }

        private void Place()
        {
            var owner = ownerRoot != null ? ownerRoot : gameObject;
            var ctx = new ToolContext(
                aimOrigin.position,
                aimOrigin.forward,
                config.PlacementRange,
                owner,
                config.PlacementMask);
            placeTool.TryUse(ctx);
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
