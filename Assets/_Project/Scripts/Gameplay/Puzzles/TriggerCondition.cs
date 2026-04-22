using System;
using PhysicsHeist.Core.Puzzles;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Puzzles
{
    [DisallowMultipleComponent]
    public sealed class TriggerCondition : MonoBehaviour, ICondition
    {
        [SerializeField] private MonoBehaviour triggerRef;
        [SerializeField] private bool requiredState = true;

        private ITrigger _trigger;
        private bool _lastEval;

        public event Action<ICondition> Changed;

        public bool Evaluate()
        {
            return _trigger != null && _trigger.IsActive == requiredState;
        }

        private void OnEnable()
        {
            _trigger = triggerRef as ITrigger;
            if (_trigger == null) return;

            _trigger.Activated += OnTriggerStateChanged;
            _trigger.Deactivated += OnTriggerStateChanged;
            _lastEval = Evaluate();
        }

        private void OnDisable()
        {
            if (_trigger == null) return;
            _trigger.Activated -= OnTriggerStateChanged;
            _trigger.Deactivated -= OnTriggerStateChanged;
            _trigger = null;
        }

        private void OnTriggerStateChanged(ITrigger _)
        {
            var current = Evaluate();
            if (current == _lastEval) return;
            _lastEval = current;
            Changed?.Invoke(this);
        }

        private void OnValidate()
        {
            if (triggerRef != null && triggerRef is not ITrigger)
            {
                Debug.LogWarning(
                    $"[{name}] Assigned triggerRef ({triggerRef.GetType().Name}) does not implement ITrigger.",
                    this);
                triggerRef = null;
            }
        }
    }
}
