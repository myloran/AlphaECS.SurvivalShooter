using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

namespace AlphaECS.Unity
{
    public abstract class ComponentBehaviour : MonoBehaviour, IDisposable
    {
        [Inject]
        protected IEventSystem EventSystem { get; set; }

        private CompositeDisposable _disposer = new CompositeDisposable();
        public CompositeDisposable Disposer
        {
            get { return _disposer; }
            private set { _disposer = value; }
        }
			
        public virtual void OnDestroy()
        {
			if (EcsApplication.IsQuitting) { return; }

            Dispose();

			if (EventSystem == null)
			{
				Debug.LogWarning("A COMPONENT ON " + this.gameObject.name + " WAS NOT INJECTED PROPERLY!");
				return;
			}
            EventSystem.Publish(new ComponentDestroyed() { Component = this });
        }

        [Inject]
		public virtual void Initialize()
        {
            EventSystem.Publish(new ComponentCreated() { Component = this });
        }

        public virtual void Dispose()
        {
            Disposer.Dispose();
        }

        public virtual void OnApplicationQuit()
        {
			EcsApplication.IsQuitting = true;
        }
    }
}
