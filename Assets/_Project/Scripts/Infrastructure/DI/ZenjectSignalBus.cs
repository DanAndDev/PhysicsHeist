using System;
using PhysicsHeist.Core.Signals;
using Zenject;

namespace PhysicsHeist.Infrastructure.DI
{
    internal sealed class ZenjectSignalBus : ISignalBus
    {
        private readonly SignalBus _bus;

        public ZenjectSignalBus(SignalBus bus)
        {
            _bus = bus;
        }

        public void Fire<TSignal>(TSignal signal) => _bus.Fire(signal);

        public void Fire<TSignal>() where TSignal : new() => _bus.Fire(new TSignal());

        public void Subscribe<TSignal>(Action<TSignal> listener) => _bus.Subscribe(listener);

        public void Unsubscribe<TSignal>(Action<TSignal> listener) => _bus.Unsubscribe(listener);
    }
}
