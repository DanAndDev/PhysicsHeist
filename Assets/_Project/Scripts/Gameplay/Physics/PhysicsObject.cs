using PhysicsHeist.Core.Physics;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public class PhysicsObject : MonoBehaviour, IPhysicsInteractable, IForceReceiver
    {
        [SerializeField] private PhysicsObjectState state = PhysicsObjectState.Dynamic;

        private Rigidbody _rigidbody;

        public Transform Transform => transform;
        public PhysicsObjectState State => state;
        public bool IsActive => isActiveAndEnabled;
        public Rigidbody Rigidbody => _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            ApplyState();
        }

        public void SetState(PhysicsObjectState state)
        {
            if (this.state == state) return;
            this.state = state;
            ApplyState();
        }

        public virtual void ApplyForce(Vector3 force, ForceMode mode = ForceMode.Force)
        {
            if (!CanReceiveForce()) return;
            _rigidbody.AddForce(force, mode);
        }

        public virtual void ApplyForceAtPoint(Vector3 force, Vector3 worldPoint, ForceMode mode = ForceMode.Force)
        {
            if (!CanReceiveForce()) return;
            _rigidbody.AddForceAtPosition(force, worldPoint, mode);
        }

        public virtual void ApplyRadialForce(Vector3 origin, float magnitude, float radius, ForceMode mode = ForceMode.Impulse)
        {
            if (!CanReceiveForce() || radius <= 0f) return;

            var toSelf = _rigidbody.worldCenterOfMass - origin;
            var distance = toSelf.magnitude;
            if (distance > radius) return;

            var falloff = 1f - (distance / radius);
            var direction = distance > 1e-4f ? toSelf / distance : Vector3.up;
            _rigidbody.AddForce(direction * (magnitude * falloff), mode);
        }

        private bool CanReceiveForce()
        {
            return _rigidbody != null && !_rigidbody.isKinematic && isActiveAndEnabled;
        }

        private void ApplyState()
        {
            if (_rigidbody == null) return;

            switch (state)
            {
                case PhysicsObjectState.Static:
                    _rigidbody.isKinematic = true;
                    _rigidbody.useGravity = false;
                    _rigidbody.constraints = RigidbodyConstraints.None;
                    break;
                case PhysicsObjectState.Dynamic:
                    _rigidbody.isKinematic = false;
                    _rigidbody.useGravity = true;
                    _rigidbody.constraints = RigidbodyConstraints.None;
                    break;
                case PhysicsObjectState.Constrained:
                    _rigidbody.isKinematic = false;
                    _rigidbody.useGravity = true;
                    break;
            }
        }
    }
}
