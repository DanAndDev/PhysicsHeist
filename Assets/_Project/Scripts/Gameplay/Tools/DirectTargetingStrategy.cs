using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// A targeting strategy that never raycasts — it always returns a valid
    /// target at the aim origin, with normal opposite to the aim direction.
    /// Used by execution strategies that don't need a hit surface (e.g.
    /// throwing a physics projectile): the <see cref="Tool"/> flow still
    /// runs cleanly because it always sees a valid target.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DirectTargetingStrategy : MonoBehaviour, ITargetingStrategy
    {
        public ToolTarget Resolve(in ToolContext context)
        {
            return new ToolTarget(
                point: context.Origin,
                normal: -context.Direction,
                distance: 0f,
                collider: null,
                rigidbody: null,
                interactable: null);
        }
    }
}
