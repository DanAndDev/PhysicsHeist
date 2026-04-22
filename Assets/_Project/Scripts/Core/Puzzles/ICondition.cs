using System;

namespace PhysicsHeist.Core.Puzzles
{
    public interface ICondition
    {
        bool Evaluate();

        event Action<ICondition> Changed;
    }
}
