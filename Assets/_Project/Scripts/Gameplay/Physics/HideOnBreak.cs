using PhysicsHeist.Core.Physics;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Physics
{
    /// <summary>
    /// Deactivates a set of GameObjects when a <see cref="BreakableObject"/> breaks.
    /// Lets a level design flag pieces of a "breakable" as cosmetic-only — the
    /// Broken event isn't destructive on its own; this bridges the decision
    /// about what to physically remove to the designer instead of baking it
    /// into the IBreakable contract.
    ///
    /// Leave <see cref="target"/> null to listen to a <see cref="BreakableObject"/>
    /// on the same GameObject.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class HideOnBreak : MonoBehaviour
    {
        [SerializeField] private BreakableObject target;
        [SerializeField] private GameObject[] objectsToHide;
        [SerializeField] private bool disableOwnColliders = true;
        [SerializeField] private bool disableOwnRenderers = true;

        private void Reset()
        {
            target = GetComponent<BreakableObject>();
        }

        private void OnEnable()
        {
            if (target == null) target = GetComponent<BreakableObject>();
            if (target == null) return;
            target.Broken += OnBroken;
        }

        private void OnDisable()
        {
            if (target != null) target.Broken -= OnBroken;
        }

        private void OnBroken(IBreakable _, BreakageInfo __)
        {
            if (objectsToHide != null)
            {
                foreach (var go in objectsToHide)
                    if (go != null) go.SetActive(false);
            }

            if (disableOwnColliders)
            {
                foreach (var col in GetComponentsInChildren<Collider>(includeInactive: false))
                    col.enabled = false;
            }

            if (disableOwnRenderers)
            {
                foreach (var rend in GetComponentsInChildren<Renderer>(includeInactive: false))
                    rend.enabled = false;
            }
        }
    }
}
