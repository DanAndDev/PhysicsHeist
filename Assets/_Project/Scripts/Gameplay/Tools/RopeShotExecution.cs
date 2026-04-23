using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [DisallowMultipleComponent]
    public sealed class RopeShotExecution : MonoBehaviour, IExecutionStrategy
    {
        [SerializeField] private RopeLauncherConfig config;
        [SerializeField] private Rope ropePrefab;
        [SerializeField] private Transform visualOrigin;

        private Rope _activeRope;

        public bool HasActiveRope => _activeRope != null && _activeRope.IsAttached;

        public void Execute(in ToolContext context, in ToolTarget target)
        {
            if (config == null || ropePrefab == null || context.Owner == null) return;
            if (!target.Valid) return;

            var ownerBody = context.Owner.GetComponent<Rigidbody>();
            if (ownerBody == null) return;

            ReleaseActive();

            var anchorTransform = target.Collider != null ? target.Collider.transform : null;
            var length = Mathf.Clamp(target.Distance, config.MinLength, config.MaxLength);

            var instance = Object.Instantiate(ropePrefab);
            instance.transform.SetParent(null);
            instance.Attach(
                ownerBody,
                visualOrigin,
                anchorTransform,
                target.Rigidbody,
                target.Point,
                config.Spring,
                config.Damper,
                length);
            instance.Released += OnRopeReleased;

            _activeRope = instance;
        }

        public void ReleaseActive()
        {
            if (_activeRope != null)
                _activeRope.Release();
        }

        public void WindIn(float delta)
        {
            if (_activeRope == null || config == null) return;
            var target = Mathf.Max(config.MinLength, _activeRope.GetMaxDistance() - delta);
            _activeRope.SetMaxDistance(target);
        }

        private void OnRopeReleased(Rope rope)
        {
            rope.Released -= OnRopeReleased;
            if (_activeRope == rope) _activeRope = null;
        }

        private void OnDisable()
        {
            ReleaseActive();
        }
    }
}
