using UnityEngine;

namespace PhysicsHeist.Core.Input
{
    public interface IInputService
    {
        Vector2 Move { get; }
        Vector2 Look { get; }
        bool JumpPressedThisFrame { get; }
        bool JumpHeld { get; }
        bool PrimaryFirePressedThisFrame { get; }
        bool PrimaryFireHeld { get; }
        bool SecondaryFirePressedThisFrame { get; }
        bool SecondaryFireHeld { get; }

        /// <summary>
        /// True on the frame a "cycle to next tool" gesture was performed
        /// (Tab or mouse scroll). ToolSlot consumes this.
        /// </summary>
        bool ToolCyclePressedThisFrame { get; }

        /// <summary>
        /// Zero-based slot index selected this frame via number keys 1..N,
        /// or -1 if no slot was explicitly picked. Returns the lowest-numbered
        /// key pressed on this frame if several were pressed simultaneously.
        /// </summary>
        int ToolSlotSelectedThisFrame { get; }
    }
}
