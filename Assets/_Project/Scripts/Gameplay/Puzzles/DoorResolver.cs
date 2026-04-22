using PhysicsHeist.Core.Puzzles;
using PhysicsHeist.Gameplay.Environment;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Puzzles
{
    [DisallowMultipleComponent]
    public sealed class DoorResolver : MonoBehaviour, IResolver
    {
        [SerializeField] private LockedDoor door;
        [SerializeField] private bool openOnResolve = true;
        [SerializeField] private bool lockOnUnresolve;

        public void Resolve()
        {
            if (door == null) return;
            if (openOnResolve) door.Open();
            else door.Close();
        }

        public void Unresolve()
        {
            if (door == null) return;
            if (openOnResolve) door.Close();
            else door.Open();
            if (lockOnUnresolve) door.SetLocked(true);
        }
    }
}
