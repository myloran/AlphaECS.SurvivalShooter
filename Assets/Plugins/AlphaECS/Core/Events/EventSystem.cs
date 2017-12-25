using System;
using UniRx;

namespace AlphaECS
{
    public class EventSystem : IEventSystem
    {
        public IMessageBroker MessageBroker { get; private set; }

        public EventSystem(IMessageBroker messageBroker)
        { MessageBroker = messageBroker; }

        public void Publish<T>(T message)
        { MessageBroker.Publish(message); }

        public IObservable<T> OnEvent<T>()
        { return MessageBroker.Receive<T>(); }

        //public IDisposable OnEvent<T>(Action<BaseEvent<object, object, object, object>, T1, T2, T3, T4> action) where T : BaseEvent<object, object, object, object> {
        //    return MessageBroker.Receive<T>().Subscribe(T => );
        //}
    }
}