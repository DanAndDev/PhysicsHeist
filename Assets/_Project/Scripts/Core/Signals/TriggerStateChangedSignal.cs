using PhysicsHeist.Core.Puzzles;

namespace PhysicsHeist.Core.Signals
{
    public readonly struct TriggerStateChangedSignal
    {
        public readonly ITrigger Trigger;
        public readonly bool IsActive;

        public TriggerStateChangedSignal(ITrigger trigger, bool isActive)
        {
            Trigger = trigger;
            IsActive = isActive;
        }
    }
}
