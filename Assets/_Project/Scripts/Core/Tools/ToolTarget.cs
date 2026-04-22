using PhysicsHeist.Core.Physics;
using UnityEngine;

namespace PhysicsHeist.Core.Tools
{
    public readonly struct ToolTarget
    {
        public readonly bool Valid;
        public readonly Vector3 Point;
        public readonly Vector3 Normal;
        public readonly float Distance;
        public readonly Collider Collider;
        public readonly Rigidbody Rigidbody;
        public readonly IPhysicsInteractable Interactable;

        public static ToolTarget None => default;

        public ToolTarget(
            Vector3 point,
            Vector3 normal,
            float distance,
            Collider collider,
            Rigidbody rigidbody,
            IPhysicsInteractable interactable)
        {
            Valid = true;
            Point = point;
            Normal = normal;
            Distance = distance;
            Collider = collider;
            Rigidbody = rigidbody;
            Interactable = interactable;
        }
    }
}
