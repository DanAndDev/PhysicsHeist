using System;
using UnityEngine;

namespace PhysicsHeist.Core.Events
{
    public abstract class EventChannelSO<T> : ScriptableObject, IEventChannel<T>
    {
        private event Action<T> Listeners;

        public void Raise(T payload)
        {
            Listeners?.Invoke(payload);
        }

        public void Subscribe(Action<T> listener)
        {
            Listeners += listener;
        }

        public void Unsubscribe(Action<T> listener)
        {
            Listeners -= listener;
        }
    }
}
