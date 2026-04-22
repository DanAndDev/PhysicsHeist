using PhysicsHeist.Core.Puzzles;

namespace PhysicsHeist.Core.Signals
{
    public readonly struct PuzzleSolvedSignal
    {
        public readonly IPuzzle Puzzle;
        public readonly bool IsSolved;

        public PuzzleSolvedSignal(IPuzzle puzzle, bool isSolved)
        {
            Puzzle = puzzle;
            IsSolved = isSolved;
        }
    }
}
