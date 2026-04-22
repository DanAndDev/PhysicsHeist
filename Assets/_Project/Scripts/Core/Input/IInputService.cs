using UnityEngine;

namespace PhysicsHeist.Core.Input
{
    public interface IInputService
    {
        Vector2 Move { get; }
        Vector2 Look { get; }
        bool JumpPressedThisFrame { get; }
        bool JumpHeld { get; }
    }
}
