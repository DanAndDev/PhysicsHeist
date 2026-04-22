using PhysicsHeist.Core.Input;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Player
{
    [DisallowMultipleComponent]
    public sealed class PlayerLook : MonoBehaviour
    {
        [SerializeField] private PlayerConfig config;
        [SerializeField] private Transform yawBody;
        [SerializeField] private bool lockCursor = true;

        private IInputService _input;
        private float _pitch;

        [Inject]
        private void Construct(IInputService input)
        {
            _input = input;
        }

        private void OnEnable()
        {
            if (!lockCursor) return;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            if (!lockCursor) return;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            if (_input == null || config == null || yawBody == null) return;

            var delta = _input.Look * config.mouseSensitivity;

            yawBody.Rotate(0f, delta.x, 0f, Space.World);

            _pitch = Mathf.Clamp(_pitch - delta.y, config.minPitch, config.maxPitch);
            transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
