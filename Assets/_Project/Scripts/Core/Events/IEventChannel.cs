using System;

namespace PhysicsHeist.Core.Events
{
    public interface IEventChannel<T>
    {
        void Raise(T payload);
        void Subscribe(Action<T> listener);
        void Unsubscribe(Action<T> listener);
    }
}
