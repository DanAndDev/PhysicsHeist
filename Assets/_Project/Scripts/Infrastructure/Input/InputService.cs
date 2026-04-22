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
        }

        public Vector2 Move => _move.ReadValue<Vector2>();
        public Vector2 Look => _look.ReadValue<Vector2>();
        public bool JumpPressedThisFrame => _jump.WasPressedThisFrame();
        public bool JumpHeld => _jump.IsPressed();

        public void Initialize()
        {
            _move.Enable();
            _look.Enable();
            _jump.Enable();
        }

        public void Dispose()
        {
            _move.Dispose();
            _look.Dispose();
            _jump.Dispose();
        }
    }
}
