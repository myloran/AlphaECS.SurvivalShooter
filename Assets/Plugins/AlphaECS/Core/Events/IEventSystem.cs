using System;
using UniRx;

namespace AlphaECS {
    public interface IEventSystem {
        void Publish<T>(T message);
        IObservable<T> OnEvent<T>();
        IDisposable On<T>(Action<T> action) where T : IEvent;
        IDisposable On<C, T>(Action<C, T> action) where C : class where T : IEvent;
        IDisposable On<C1, C2, T>(Action<C1, C2, T> action) where C1 : class
            where C2: class where T : IEvent;
        IDisposable On<C1, C2, C3, T>(Action<C1, C2, C3, T> action) where C1 : class
            where C2 : class where C3 : class where T : IEvent;
        IDisposable On<C1, C2, C3, C4, T>(Action<C1, C2, C3, C4, T> action) where C1 : class
            where C2 : class where C3 : class where C4 : class where T : IEvent;
        IDisposable On<C1, C2, C3, C4, C5, T>(Action<C1, C2, C3, C4, C5, T> action) where C1 : class
            where C2 : class where C3 : class where C4 : class where C5 : class where T : IEvent;
        IDisposable On<C1, C2, C3, C4, C5, C6, T>(Action<C1, C2, C3, C4, C5, C6, T> action) 
            where C1 : class where C2 : class where C3 : class where C4 : class where C5 : class 
            where C6 : class where T : IEvent;
        IDisposable On<C1, C2, C3, C4, C5, C6, C7, T>(Action<C1, C2, C3, C4, C5, C6, C7, T> action) 
            where C1 : class where C2 : class where C3 : class where C4 : class where C5 : class 
            where C6 : class where C7 : class where T : IEvent;
    }
}