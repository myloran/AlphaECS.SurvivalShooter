using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaECS
{
	public interface IEntity : IDisposable
    {
        //add event here, so that every group interested in this event receive it and after that disposes itself automatically
        //T Publish<T>() where T : class, new();
        
        string Id { get; }
		IEnumerable<object> Components { get; }

		object Add(object component);
		T Add<T> () where T : class, new(); 
		void Remove(object component);
        void Remove<T>() where T : class;
        void RemoveAll();
        T Get<T>() where T : class;
		object Get (Type type);

        bool Has<T>() where T : class;
		bool Has(Type componentType);
        bool Has(params Type[] component);
    }
}
