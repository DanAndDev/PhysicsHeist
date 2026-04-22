using PhysicsHeist.Core.Input;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerConfig config;
        [SerializeField] private Transform groundCheckOrigin;

        private IInputService _input;
        private Rigidbody _rigidbody;
        private bool _jumpQueued;
        private bool _isGrounded;

        [Inject]
        private void Construct(IInputService input)
        {
            _input = input;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        private void Update()
        {
            if (_input == null || config == null) return;
            if (_input.JumpPressedThisFrame) _jumpQueued = true;
        }

        private void FixedUpdate()
        {
            if (_input == null || config == null) return;

            _isGrounded = CheckGrounded();

            ApplyMovement();
            ApplyJump();
        }

        private void ApplyMovement()
        {
            var input = _input.Move;
            var planarInput = new Vector3(input.x, 0f, input.y);
            if (planarInput.sqrMagnitude > 1f) planarInput.Normalize();

            var yaw = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            var desired = yaw * planarInput * config.moveSpeed;

            var velocity = _rigidbody.velocity;
            var planar = new Vector3(velocity.x, 0f, velocity.z);

            var controlFactor = _isGrounded ? 1f : config.airControlMultiplier;
            var next = Vector3.MoveTowards(planar, desired, config.acceleration * controlFactor * Time.fixedDeltaTime);

            _rigidbody.velocity = new Vector3(next.x, velocity.y, next.z);
        }

        private void ApplyJump()
        {
            if (!_jumpQueued) return;

            if (_isGrounded)
            {
                var velocity = _rigidbody.velocity;
                velocity.y = 0f;
                _rigidbody.velocity = velocity;
                _rigidbody.AddForce(Vector3.up * config.jumpImpulse, ForceMode.VelocityChange);
            }

            _jumpQueued = false;
        }

        private bool CheckGrounded()
        {
            var origin = groundCheckOrigin != null
                ? groundCheckOrigin.position
                : transform.position + Vector3.up * 0.1f;

            return UnityEngine.Physics.SphereCast(
                origin,
                config.groundCheckRadius,
                Vector3.down,
                out _,
                config.groundCheckDistance,
                config.groundMask,
                QueryTriggerInteraction.Ignore);
        }
    }
}
