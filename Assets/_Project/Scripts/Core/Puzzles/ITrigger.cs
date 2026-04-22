using System;

namespace PhysicsHeist.Core.Puzzles
{
    public interface ITrigger
    {
        bool IsActive { get; }

        event Action<ITrigger> Activated;
        event Action<ITrigger> Deactivated;
    }
}
