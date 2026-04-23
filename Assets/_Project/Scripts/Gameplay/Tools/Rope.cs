using System;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [RequireComponent(typeof(LineRenderer))]
    [DisallowMultipleComponent]
    public sealed class Rope : MonoBehaviour
    {
        private LineRenderer _line;
        private SpringJoint _joint;

        private Rigidbody _ownerBody;
        private Transform _visualOrigin;
        private Transform _anchorTransform;
        private Vector3 _anchorLocal;
        private Vector3 _anchorWorldIfStatic;
        private bool _anchorStatic;
        private bool _released;

        public event Action<Rope> Released;

        public bool IsAttached => _joint != null && !_released;

        private void Awake()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = true;
        }

        public void Attach(
            Rigidbody ownerBody,
            Transform visualOrigin,
            Transform anchorTransform,
            Rigidbody anchorBody,
            Vector3 anchorWorldPoint,
            float springForce,
            float damper,
            float maxDistance)
        {
            if (ownerBody == null)
            {
                Destroy(gameObject);
                return;
            }

            _ownerBody = ownerBody;
            _visualOrigin = visualOrigin != null ? visualOrigin : ownerBody.transform;
            _anchorTransform = anchorTransform;
            _anchorStatic = anchorBody == null;

            if (_anchorStatic)
            {
                _anchorWorldIfStatic = anchorWorldPoint;
                _anchorLocal = Vector3.zero;
            }
            else
            {
                _anchorLocal = anchorTransform.InverseTransformPoint(anchorWorldPoint);
                _anchorWorldIfStatic = default;
            }

            _joint = ownerBody.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.anchor = Vector3.zero;
            _joint.connectedBody = anchorBody;
            _joint.connectedAnchor = _anchorStatic ? _anchorWorldIfStatic : _anchorLocal;
            _joint.spring = springForce;
            _joint.damper = damper;
            _joint.minDistance = 0f;
            _joint.maxDistance = Mathf.Max(0.1f, maxDistance);
            _joint.tolerance = 0.01f;
            _joint.enableCollision = false;
        }

        public float GetMaxDistance() => _joint != null ? _joint.maxDistance : 0f;

        public void SetMaxDistance(float distance)
        {
            if (_joint == null) return;
            _joint.maxDistance = Mathf.Max(0.1f, distance);
        }

        public void Release()
        {
            if (_released) return;
            _released = true;

            if (_joint != null)
            {
                Destroy(_joint);
                _joint = null;
            }

            Released?.Invoke(this);
            Destroy(gameObject);
        }

        private void LateUpdate()
        {
            if (_released) return;

            var anchorValid = _anchorStatic || _anchorTransform != null;
            if (_ownerBody == null || !anchorValid || _joint == null)
            {
                Release();
                return;
            }

            var ownerWorld = _visualOrigin != null ? _visualOrigin.position : _ownerBody.position;
            var anchorWorld = _anchorStatic
                ? _anchorWorldIfStatic
                : _anchorTransform.TransformPoint(_anchorLocal);

            _line.SetPosition(0, ownerWorld);
            _line.SetPosition(1, anchorWorld);
        }
    }
}
