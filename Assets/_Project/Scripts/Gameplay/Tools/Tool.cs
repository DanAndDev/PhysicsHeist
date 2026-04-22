using System;
using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [DisallowMultipleComponent]
    public class Tool : MonoBehaviour, IToolAction
    {
        [SerializeField] private string displayName = "Tool";
        [SerializeField] private MonoBehaviour targetingStrategy;
        [SerializeField] private MonoBehaviour executionStrategy;

        private ITargetingStrategy _targeting;
        private IExecutionStrategy _execution;

        public string DisplayName => displayName;
        public virtual bool IsReady => _targeting != null && _execution != null && isActiveAndEnabled;

        public event Action<IToolAction, ToolUseResult, ToolTarget> Used;

        protected virtual void Awake()
        {
            _targeting = targetingStrategy as ITargetingStrategy;
            _execution = executionStrategy as IExecutionStrategy;

            if (_targeting == null)
                Debug.LogWarning($"[{name}] Targeting strategy is missing or does not implement ITargetingStrategy.", this);
            if (_execution == null)
                Debug.LogWarning($"[{name}] Execution strategy is missing or does not implement IExecutionStrategy.", this);
        }

        public ToolUseResult TryUse(in ToolContext context)
        {
            if (!IsReady)
                return Report(ToolUseResult.Disabled, ToolTarget.None);

            var target = _targeting.Resolve(in context);
            if (!target.Valid)
                return Report(ToolUseResult.NoTarget, target);

            var result = Execute(in context, in target);
            return Report(result, target);
        }

        protected virtual ToolUseResult Execute(in ToolContext context, in ToolTarget target)
        {
            _execution.Execute(in context, in target);
            return ToolUseResult.Success;
        }

        private ToolUseResult Report(ToolUseResult result, ToolTarget target)
        {
            Used?.Invoke(this, result, target);
            return result;
        }

        protected virtual void OnValidate()
        {
            if (targetingStrategy != null && targetingStrategy is not ITargetingStrategy)
            {
                Debug.LogWarning(
                    $"[{name}] Assigned targetingStrategy ({targetingStrategy.GetType().Name}) does not implement ITargetingStrategy.",
                    this);
                targetingStrategy = null;
            }

            if (executionStrategy != null && executionStrategy is not IExecutionStrategy)
            {
                Debug.LogWarning(
                    $"[{name}] Assigned executionStrategy ({executionStrategy.GetType().Name}) does not implement IExecutionStrategy.",
                    this);
                executionStrategy = null;
            }
        }
    }
}
