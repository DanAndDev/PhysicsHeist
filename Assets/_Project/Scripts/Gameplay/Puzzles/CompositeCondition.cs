using System;
using System.Collections.Generic;
using PhysicsHeist.Core.Puzzles;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Puzzles
{
    public enum CompositeOperator
    {
        And,
        Or,
    }

    [DisallowMultipleComponent]
    public sealed class CompositeCondition : MonoBehaviour, ICondition
    {
        [SerializeField] private CompositeOperator op = CompositeOperator.And;
        [SerializeField] private bool invert;
        [SerializeField] private MonoBehaviour[] conditionRefs;

        private readonly List<ICondition> _children = new();
        private bool _lastEval;

        public event Action<ICondition> Changed;

        public bool Evaluate()
        {
            var result = EvaluateChildren();
            return invert ? !result : result;
        }

        private bool EvaluateChildren()
        {
            if (_children.Count == 0)
                return op == CompositeOperator.And;

            for (var i = 0; i < _children.Count; i++)
            {
                var v = _children[i].Evaluate();
                if (op == CompositeOperator.And && !v) return false;
                if (op == CompositeOperator.Or && v) return true;
            }

            return op == CompositeOperator.And;
        }

        private void OnEnable()
        {
            _children.Clear();
            if (conditionRefs != null)
            {
                foreach (var mb in conditionRefs)
                {
                    if (mb is ICondition c)
                    {
                        _children.Add(c);
                        c.Changed += OnChildChanged;
                    }
                }
            }
            _lastEval = Evaluate();
        }

        private void OnDisable()
        {
            foreach (var c in _children) c.Changed -= OnChildChanged;
            _children.Clear();
        }

        private void OnChildChanged(ICondition _)
        {
            var current = Evaluate();
            if (current == _lastEval) return;
            _lastEval = current;
            Changed?.Invoke(this);
        }

        private void OnValidate()
        {
            if (conditionRefs == null) return;
            for (var i = 0; i < conditionRefs.Length; i++)
            {
                var mb = conditionRefs[i];
                if (mb != null && mb is not ICondition)
                {
                    Debug.LogWarning(
                        $"[{name}] conditionRefs[{i}] ({mb.GetType().Name}) does not implement ICondition.",
                        this);
                    conditionRefs[i] = null;
                }
            }
        }
    }
}
