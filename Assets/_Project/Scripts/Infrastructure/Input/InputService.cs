using System;
using PhysicsHeist.Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace PhysicsHeist.Infrastructure.Input
{
    internal sealed class InputService : IInputService, IInitializable, IDisposable
    {
        private const int ToolSlotCount = 3;

        private readonly InputAction _move;
        private readonly InputAction _look;
        private readonly InputAction _jump;
        private readonly InputAction _primaryFire;
        private readonly InputAction _secondaryFire;
        private readonly InputAction _toolCycle;
        private readonly InputAction[] _toolSlots;

        public InputService()
        {
            _move = new InputAction("Move", InputActionType.Value);
            _move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");

            _look = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");

            _jump = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");

            _primaryFire = new InputAction("PrimaryFire", InputActionType.Button, "<Mouse>/leftButton");
            _secondaryFire = new InputAction("SecondaryFire", InputActionType.Button, "<Mouse>/rightButton");

            // Tab cycles tools; scroll wheel is bound as a second path so either
            // works. We use the scroll "up" direction so a single notch up
            // triggers one cycle; scroll "down" is left for future reverse-cycle.
            _toolCycle = new InputAction("ToolCycle", InputActionType.Button);
            _toolCycle.AddBinding("<Keyboard>/tab");
            _toolCycle.AddBinding("<Mouse>/scroll/y").WithProcessor("scaleVector2(x=0,y=1)");

            _toolSlots = new InputAction[ToolSlotCount];
            for (var i = 0; i < ToolSlotCount; i++)
            {
                var keyDigit = (i + 1).ToString();
                _toolSlots[i] = new InputAction(
                    $"ToolSlot{i}",
                    InputActionType.Button,
                    $"<Keyboard>/{keyDigit}");
            }
        }

        public Vector2 Move => _move.ReadValue<Vector2>();
        public Vector2 Look => _look.ReadValue<Vector2>();
        public bool JumpPressedThisFrame => _jump.WasPressedThisFrame();
        public bool JumpHeld => _jump.IsPressed();
        public bool PrimaryFirePressedThisFrame => _primaryFire.WasPressedThisFrame();
        public bool PrimaryFireHeld => _primaryFire.IsPressed();
        public bool SecondaryFirePressedThisFrame => _secondaryFire.WasPressedThisFrame();
        public bool SecondaryFireHeld => _secondaryFire.IsPressed();
        public bool ToolCyclePressedThisFrame => _toolCycle.WasPressedThisFrame();

        public int ToolSlotSelectedThisFrame
        {
            get
            {
                for (var i = 0; i < _toolSlots.Length; i++)
                    if (_toolSlots[i].WasPressedThisFrame())
                        return i;
                return -1;
            }
        }

        public void Initialize()
        {
            _move.Enable();
            _look.Enable();
            _jump.Enable();
            _primaryFire.Enable();
            _secondaryFire.Enable();
            _toolCycle.Enable();
            foreach (var slot in _toolSlots) slot.Enable();
        }

        public void Dispose()
        {
            _move.Dispose();
            _look.Dispose();
            _jump.Dispose();
            _primaryFire.Dispose();
            _secondaryFire.Dispose();
            _toolCycle.Dispose();
            foreach (var slot in _toolSlots) slot.Dispose();
        }
    }
}
