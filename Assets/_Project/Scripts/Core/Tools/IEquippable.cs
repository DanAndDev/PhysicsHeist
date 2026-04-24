namespace PhysicsHeist.Core.Tools
{
    /// <summary>
    /// A tool (or any item) that can be held in a <see cref="ToolSlot"/>-style
    /// container. Exactly one equippable is active at a time per slot; swapping
    /// calls <see cref="Unequip"/> on the outgoing item and <see cref="Equip"/>
    /// on the incoming one.
    ///
    /// The contract is intentionally minimal — implementations decide what
    /// "equipped" means (visibility, input routing, physics state, etc.).
    /// Keeping it at the Core layer lets Gameplay controllers depend on the
    /// abstraction without pulling in specific tool classes.
    /// </summary>
    public interface IEquippable
    {
        /// <summary>Human-readable name for UI / HUD / debug.</summary>
        string DisplayName { get; }

        /// <summary>True while this item is the active selection in its slot.</summary>
        bool IsEquipped { get; }

        /// <summary>Called by the slot when this item becomes active.</summary>
        void Equip();

        /// <summary>Called by the slot when this item is deselected.</summary>
        void Unequip();
    }
}
