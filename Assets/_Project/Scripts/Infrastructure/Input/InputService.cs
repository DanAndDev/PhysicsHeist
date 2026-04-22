using System;
using PhysicsHeist.Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace PhysicsHeist.Infrastructure.Input
{
    internal sealed class InputService : IInputService, IInitializable, IDisposable
    {
        private readonly InputAction _move;
        private readonly InputAction _look;
        private readonly InputAction _jump;
        private readonly InputAction _primaryFire;
        private readonly InputAction _secondaryFire;

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
        }

        public Vector2 Move => _move.ReadValue<Vector2>();
        public Vector2 Look => _look.ReadValue<Vector2>();
        public bool JumpPressedThisFrame => _jump.WasPressedThisFrame();
        public bool JumpHeld => _jump.IsPressed();
        public bool PrimaryFirePressedThisFrame => _primaryFire.WasPressedThisFrame();
        public bool PrimaryFireHeld => _primaryFire.IsPressed();
        public bool SecondaryFirePressedThisFrame => _secondaryFire.WasPressedThisFrame();
        public bool SecondaryFireHeld => _secondaryFire.IsPressed();

        public void Initialize()
        {
            _move.Enable();
            _look.Enable();
            _jump.Enable();
            _primaryFire.Enable();
            _secondaryFire.Enable();
        }

        public void Dispose()
        {
            _move.Dispose();
            _look.Dispose();
            _jump.Dispose();
            _primaryFire.Dispose();
            _secondaryFire.Dispose();
        }
    }
}
