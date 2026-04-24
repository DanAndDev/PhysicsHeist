using PhysicsHeist.Core.Input;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public sealed class PlayerController : MonoBehaviour
    {
        // Shared across all PlayerController instances — grounding is resolved
        // synchronously inside FixedUpdate, so reuse is safe.
        private static readonly Collider[] GroundOverlapBuffer = new Collider[8];

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
            // We want an overlap test, not a sweep — Physics.SphereCast ignores
            // initial overlaps, which is why the earlier version never fired:
            // the check sphere already intersected the floor at t=0.
            //
            // The catch with a plain CheckSphere is that the ground mask
            // defaults to Everything, so the overlap also picks up the
            // player's own capsule collider (the capsule bottom sits at the
            // same height as the ground line). That would report "grounded"
            // eternally — including mid-air — so we use OverlapSphereNonAlloc
            // and skip any collider that belongs to the player hierarchy.
            var origin = groundCheckOrigin != null
                ? groundCheckOrigin.position
                : transform.position + Vector3.up * 0.1f;

            var count = UnityEngine.Physics.OverlapSphereNonAlloc(
                origin,
                config.groundCheckRadius,
                GroundOverlapBuffer,
                config.groundMask,
                QueryTriggerInteraction.Ignore);

            var grounded = false;
            for (var i = 0; i < count; i++)
            {
                var col = GroundOverlapBuffer[i];
                GroundOverlapBuffer[i] = null;
                if (col == null) continue;
                if (col.transform.IsChildOf(transform)) continue;
                grounded = true;
            }
            return grounded;
        }
    }
}
