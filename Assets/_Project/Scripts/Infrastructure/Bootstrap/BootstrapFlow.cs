using System;
using PhysicsHeist.Core.Signals;
using UnityEngine;
using Zenject;

namespace PhysicsHeist.Infrastructure.Bootstrap
{
    internal sealed class BootstrapFlow : IInitializable, IDisposable
    {
        private readonly ISignalBus _signalBus;

        public BootstrapFlow(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BootCompletedSignal>(OnBootCompleted);
            _signalBus.Fire<BootCompletedSignal>();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<BootCompletedSignal>(OnBootCompleted);
        }

        private static void OnBootCompleted(BootCompletedSignal _)
        {
            Debug.Log("[PhysicsHeist] DI + signal bus ok");
        }
    }
}
