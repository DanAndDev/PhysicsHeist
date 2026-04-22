using UnityEngine;

namespace PhysicsHeist.Core.Physics
{
    public interface IPhysicsInteractable
    {
        Transform Transform { get; }
        PhysicsObjectState State { get; }
        bool IsActive { get; }
    }
}
