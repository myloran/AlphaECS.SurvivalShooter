using AlphaECS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEvent<T> where T : class {

}

public class BaseEvent<T1, T2, T3, T4> where T1 : IEntity where T2 : IEntity where T3 : IEntity where T4 : IEntity {
    public IEventSystem EventSystem { get; set; }
    T1 t1;
    T2 t2;
    T3 t3;
    T4 t4;

    //public IDisposable OnPublish<T>(Action<T1, T2, T3, T4> action) where T: BaseEvent<T1, T2, T3, T4> {
    //    return EventSystem.OnEvent<T>().Subscribe(entity => action(entity t1, t2, t3, t4));
    //}
}
