using System;

namespace PhysicsHeist.Core.Signals
{
    public interface ISignalBus
    {
        void Fire<TSignal>(TSignal signal);
        void Fire<TSignal>() where TSignal : new();
        void Subscribe<TSignal>(Action<TSignal> listener);
        void Unsubscribe<TSignal>(Action<TSignal> listener);
    }
}
