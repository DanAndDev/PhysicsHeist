using System;
using System.Collections.Generic;
using PhysicsHeist.Core.Puzzles;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Puzzles
{
    [DisallowMultipleComponent]
    public sealed class Puzzle : MonoBehaviour, IPuzzle
    {
        [SerializeField] private string puzzleName = "Puzzle";
        [SerializeField] private MonoBehaviour[] conditionRefs;
        [SerializeField] private MonoBehaviour[] resolverRefs;
        [SerializeField] private bool oneShot;

        private readonly List<ICondition> _conditions = new();
        private readonly List<IResolver> _resolvers = new();
        private bool _isSolved;
        private bool _locked;

        public string Name => puzzleName;
        public bool IsSolved => _isSolved;

        public event Action<Puzzle> Solved;
        public event Action<Puzzle> Unsolved;

        private void OnEnable()
        {
            _conditions.Clear();
            _resolvers.Clear();

            if (conditionRefs != null)
            {
                foreach (var mb in conditionRefs)
                {
                    if (mb is ICondition c)
                    {
                        _conditions.Add(c);
                        c.Changed += OnConditionChanged;
                    }
                }
            }

            if (resolverRefs != null)
            {
                foreach (var mb in resolverRefs)
                {
                    if (mb is IResolver r) _resolvers.Add(r);
                }
            }

            Evaluate();
        }

        private void OnDisable()
        {
            foreach (var c in _conditions) c.Changed -= OnConditionChanged;
            _conditions.Clear();
            _resolvers.Clear();
        }

        private void OnConditionChanged(ICondition _) => Evaluate();

        private void Evaluate()
        {
            if (_locked) return;

            var allSatisfied = AllSatisfied();
            if (allSatisfied == _isSolved) return;

            _isSolved = allSatisfied;

            if (_isSolved)
            {
                foreach (var r in _resolvers) r.Resolve();
                Solved?.Invoke(this);
                if (oneShot) _locked = true;
            }
            else
            {
                foreach (var r in _resolvers) r.Unresolve();
                Unsolved?.Invoke(this);
            }
        }

        private bool AllSatisfied()
        {
            if (_conditions.Count == 0) return false;
            foreach (var c in _conditions)
                if (!c.Evaluate()) return false;
            return true;
        }

        private void OnValidate()
        {
            ValidateRefs(conditionRefs, typeof(ICondition), nameof(conditionRefs));
            ValidateRefs(resolverRefs, typeof(IResolver), nameof(resolverRefs));
        }

        private void ValidateRefs(MonoBehaviour[] refs, Type required, string label)
        {
            if (refs == null) return;
            for (var i = 0; i < refs.Length; i++)
            {
                var mb = refs[i];
                if (mb != null && !required.IsInstanceOfType(mb))
                {
                    Debug.LogWarning(
                        $"[{name}] {label}[{i}] ({mb.GetType().Name}) does not implement {required.Name}.",
                        this);
                    refs[i] = null;
                }
            }
        }
    }
}
