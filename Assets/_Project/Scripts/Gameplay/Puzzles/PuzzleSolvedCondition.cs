using System;
using PhysicsHeist.Core.Puzzles;
using UnityEngine;

namespace PhysicsHeist.Gameplay.Puzzles
{
    /// <summary>
    /// Exposes another <see cref="Puzzle"/>'s solved state as an <see cref="ICondition"/>.
    /// Lets master puzzles compose sub-puzzle results: AND three latched sub-puzzles
    /// to unlock a vault, OR a pair of alternate approaches, etc.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PuzzleSolvedCondition : MonoBehaviour, ICondition
    {
        [SerializeField] private Puzzle puzzle;
        [SerializeField] private bool invert;

        public event Action<ICondition> Changed;

        public bool Evaluate()
        {
            if (puzzle == null) return invert;
            return invert ? !puzzle.IsSolved : puzzle.IsSolved;
        }

        private void OnEnable()
        {
            if (puzzle == null) return;
            puzzle.Solved += OnPuzzleSolved;
            puzzle.Unsolved += OnPuzzleUnsolved;
        }

        private void OnDisable()
        {
            if (puzzle == null) return;
            puzzle.Solved -= OnPuzzleSolved;
            puzzle.Unsolved -= OnPuzzleUnsolved;
        }

        private void OnPuzzleSolved(Puzzle _) => Changed?.Invoke(this);
        private void OnPuzzleUnsolved(Puzzle _) => Changed?.Invoke(this);

        private void OnValidate()
        {
            if (puzzle == this.GetComponent<Puzzle>())
            {
                Debug.LogWarning(
                    $"[{name}] PuzzleSolvedCondition references the Puzzle on its own GameObject; " +
                    "this creates a feedback loop. Point it at a different Puzzle.", this);
            }
        }
    }
}
