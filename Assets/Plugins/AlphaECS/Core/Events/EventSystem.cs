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

        public IDisposable On<T>(Action<T> action) where T : IEvent {
            return MessageBroker.Receive<T>().Subscribe(eventData =>
                action(eventData));
        }

        public IDisposable On<C, T>(Action<C, T> action) where C : class where T : IEvent {
            return MessageBroker.Receive<T>().Subscribe(eventData => 
                action(eventData.entity.Get<C>(), eventData));}

        public IDisposable On<C1, C2, T>(Action<C1, C2, T> action)
            where C1 : class where C2 : class where T : IEvent {
            return MessageBroker.Receive<T>().Subscribe(eventData =>
                action(eventData.entity.Get<C1>(), eventData.entity.Get<C2>(), eventData));
        }

        public IDisposable On<C1, C2, C3, T>(Action<C1, C2, C3, T> action)
            where C1 : class where C2 : class where C3 : class where T : IEvent {
            return MessageBroker.Receive<T>().Subscribe(eventData =>
                action(eventData.entity.Get<C1>(), eventData.entity.Get<C2>(), eventData.entity.Get<C3>(),
                eventData));
        }

        public IDisposable On<C1, C2, C3, C4, T>(Action<C1, C2, C3, C4, T> action)
            where C1 : class where C2 : class where C3 : class where C4 : class where T : IEvent {
            return MessageBroker.Receive<T>().Subscribe(eventData =>
                action(eventData.entity.Get<C1>(), eventData.entity.Get<C2>(), eventData.entity.Get<C3>(),
                eventData.entity.Get<C4>(), eventData));
        }

        public IDisposable On<C1, C2, C3, C4, C5, T>(Action<C1, C2, C3, C4, C5, T> action)
            where C1 : class where C2 : class where C3 : class where C4 : class
            where C5 : class where T : IEvent {
            return MessageBroker.Receive<T>().Subscribe(eventData =>
                action(eventData.entity.Get<C1>(), eventData.entity.Get<C2>(), eventData.entity.Get<C3>(),
                eventData.entity.Get<C4>(), eventData.entity.Get<C5>(), eventData));
        }

        public IDisposable On<C1, C2, C3, C4, C5, C6, T>(Action<C1, C2, C3, C4, C5, C6, T> action)
            where C1 : class where C2 : class where C3 : class where C4 : class
            where C5 : class where C6 : class where T : IEvent {
            return MessageBroker.Receive<T>().Subscribe(eventData =>
                action(eventData.entity.Get<C1>(), eventData.entity.Get<C2>(), eventData.entity.Get<C3>(),
                eventData.entity.Get<C4>(), eventData.entity.Get<C5>(), eventData.entity.Get<C6>(),
                eventData));
        }

        public IDisposable On<C1, C2, C3, C4, C5, C6, C7, T>(Action<C1, C2, C3, C4, C5, C6, C7, T> action)
            where C1 : class where C2 : class where C3 : class where C4 : class
            where C5 : class where C6 : class where C7 : class where T : IEvent {
            return MessageBroker.Receive<T>().Subscribe(eventData =>
                action(eventData.entity.Get<C1>(), eventData.entity.Get<C2>(), eventData.entity.Get<C3>(),
                eventData.entity.Get<C4>(), eventData.entity.Get<C5>(), eventData.entity.Get<C6>(),
                eventData.entity.Get<C7>(), eventData));}
    }
}