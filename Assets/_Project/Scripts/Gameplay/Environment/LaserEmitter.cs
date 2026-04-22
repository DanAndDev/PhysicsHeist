using System;
using PhysicsHeist.Core.Puzzles;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Environment
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class LaserEmitter : MonoBehaviour, ITrigger
    {
        [SerializeField, Min(0f)] private float maxLength = 20f;
        [SerializeField] private Transform targetReceiver;
        [SerializeField] private LayerMask blockingMask = ~0;
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color blockedColor = Color.red;

        private LineRenderer _line;
        private bool isActive;

        public bool IsActive => isActive;

        public event Action<ITrigger> Activated;
        public event Action<ITrigger> Deactivated;

        private void Awake()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = true;
        }

        private void Update()
        {
            var start = transform.position;
            var (end, reached) = ResolveBeam(start);

            _line.SetPosition(0, start);
            _line.SetPosition(1, end);
            var color = reached ? activeColor : blockedColor;
            _line.startColor = color;
            _line.endColor = color;

            SetActive(reached);
        }

        private (Vector3 end, bool reached) ResolveBeam(Vector3 start)
        {
            Vector3 direction;
            float maxDistance;

            if (targetReceiver != null)
            {
                var delta = targetReceiver.position - start;
                maxDistance = delta.magnitude;
                direction = maxDistance > 1e-4f ? delta / maxDistance : transform.forward;
            }
            else
            {
                direction = transform.forward;
                maxDistance = maxLength;
            }

            if (UnityEngine.Physics.Raycast(start, direction, out var hit, maxDistance, blockingMask, QueryTriggerInteraction.Ignore))
                return (hit.point, false);

            return (start + direction * maxDistance, true);
        }

        private void SetActive(bool value)
        {
            if (isActive == value) return;
            isActive = value;
            if (isActive) Activated?.Invoke(this);
            else Deactivated?.Invoke(this);
        }
    }
}
