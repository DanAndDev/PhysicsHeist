using PhysicsHeist.Core.Physics;
using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    [DisallowMultipleComponent]
    public sealed class RaycastTargetingStrategy : MonoBehaviour, ITargetingStrategy
    {
        [SerializeField] private QueryTriggerInteraction triggers = QueryTriggerInteraction.Ignore;
        [SerializeField] private bool requireRigidbody = true;

        public ToolTarget Resolve(in ToolContext context)
        {
            if (!UnityEngine.Physics.Raycast(
                    context.Origin,
                    context.Direction,
                    out var hit,
                    context.Range,
                    context.LayerMask,
                    triggers))
                return ToolTarget.None;

            if (context.Owner != null && hit.collider.transform.IsChildOf(context.Owner.transform))
                return ToolTarget.None;

            var rigidbody = hit.rigidbody;
            if (requireRigidbody && rigidbody == null)
                return ToolTarget.None;

            var interactable = rigidbody != null
                ? rigidbody.GetComponent<IPhysicsInteractable>()
                : hit.collider.GetComponentInParent<IPhysicsInteractable>();

            return new ToolTarget(
                hit.point,
                hit.normal,
                hit.distance,
                hit.collider,
                rigidbody,
                interactable);
        }
    }
}
