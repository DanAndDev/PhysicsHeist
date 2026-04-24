using PhysicsHeist.Core.Tools;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Tools
{
    /// <summary>
    /// Root-level marker on a tool prefab that lets a <see cref="ToolSlot"/>
    /// equip/unequip the whole tool as a unit. "Equipped" is modeled as
    /// GameObject active state — flipping it off simultaneously hides the
    /// visual, stops the controller's Update from running, and fires OnDisable
    /// on child strategies (e.g. <see cref="RopeShotExecution"/> auto-releases
    /// its active rope on unequip via OnDisable — no extra plumbing needed).
    ///
    /// Place this on the tool's root GameObject (same level as the controller).
    /// The tool should be active in the prefab so Zenject injection runs for
    /// child components during scene load; <see cref="ToolSlot"/> then
    /// deactivates the non-starting tools in its own Start().
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EquippableTool : MonoBehaviour, IEquippable
    {
        [SerializeField] private string displayName = "Tool";

        public string DisplayName => displayName;
        public bool IsEquipped => gameObject.activeSelf;

        public void Equip()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }

        public void Unequip()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
    }
}
