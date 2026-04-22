using System;
using System.Collections.Generic;
using PhysicsHeist.Core.Puzzles;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Environment
{
    [RequireComponent(typeof(Collider))]
    public sealed class PressurePlate : MonoBehaviour, ITrigger
    {
        [SerializeField, Min(0f)] private float activationMass = 10f;

        private readonly HashSet<Rigidbody> _overlapping = new();
        private bool isActive;

        public bool IsActive => isActive;
        public float CurrentMass { get; private set; }

        public event Action<ITrigger> Activated;
        public event Action<ITrigger> Deactivated;

        private void Awake()
        {
            var col = GetComponent<Collider>();
            if (!col.isTrigger)
                Debug.LogWarning($"[{name}] PressurePlate collider should be a trigger.", this);
        }

        private void OnTriggerEnter(Collider other)
        {
            var rb = other.attachedRigidbody;
            if (rb == null) return;
            if (_overlapping.Add(rb)) Evaluate();
        }

        private void OnTriggerExit(Collider other)
        {
            var rb = other.attachedRigidbody;
            if (rb == null) return;
            if (_overlapping.Remove(rb)) Evaluate();
        }

        private void FixedUpdate()
        {
            if (_overlapping.RemoveWhere(rb => rb == null) > 0) Evaluate();
        }

        private void Evaluate()
        {
            var sum = 0f;
            foreach (var rb in _overlapping)
                if (rb != null) sum += rb.mass;

            CurrentMass = sum;

            var shouldBeActive = sum >= activationMass;
            if (shouldBeActive == isActive) return;

            isActive = shouldBeActive;
            if (isActive) Activated?.Invoke(this);
            else Deactivated?.Invoke(this);
        }
    }
}
