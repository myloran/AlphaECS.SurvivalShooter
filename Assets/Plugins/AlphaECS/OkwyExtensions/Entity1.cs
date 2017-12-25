using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlphaECS;
using UniRx;

namespace AlphaECS {
    //	[Serializable]
    public class Entity<C1, C2> : IEntity where C1 : class where C2 : class {
        public Entity(C1 val) { c1 = val; }
        public Entity(C2 val) { c2 = val; }
        C1 c1 { get; set; }
        C2 c2 { get; set; }
        public static implicit operator C1(Entity<C1, C2> entity) { return entity.c1; }
        public static implicit operator C2(Entity<C1, C2> entity) { return entity.c2; }

        private readonly Dictionary<Type, object> _components;

        public IEventSystem EventSystem { get; private set; }

        public string Id { get; private set; }
        public IEnumerable<object> Components { get { return _components.Values; } }

        public Entity(string id, IEventSystem eventSystem) {
            Id = id;
            EventSystem = eventSystem;
            _components = new Dictionary<Type, object>();
            c1 = Get<C1>();
        }

        public object Add(object component) {
            //TODO not sure this should be silently returning this way... 
            //... ideally we should be returning the component and able to check for null elsewhere
            if (_components.ContainsKey(component.GetType())) {
                //				throw new Exception(string.Format("Entity already contains a component of type {0}. Returning pre-existing component.", component.GetType().Name));
                return _components[component.GetType()];
            }

            _components.Add(component.GetType(), component);
            EventSystem.Publish(new ComponentAddedEvent(this, component));
            return component;
        }

        public T Add<T>() where T : class, new() { return (T)Add(new T()); }

        public void Remove(object component) {
            if (!_components.ContainsKey(component.GetType())) { return; }

            if (component is IDisposable) { (component as IDisposable).Dispose(); }

            _components.Remove(component.GetType());
            EventSystem.Publish(new ComponentRemovedEvent(this, component));
        }

        public void Remove<T>() where T : class {
            if (!Has<T>()) { return; }

            var component = Get<T>();
            Remove(component);
        }

        public void RemoveAll() {
            var components = Components.ToArray();
            components.ForEachRun(Remove);
        }

        public bool Has<T>() where T : class { return _components.ContainsKey(typeof(T)); }

        public bool Has(Type componentType) {
            return _components.ContainsKey(componentType);
        }

        public bool Has(params Type[] componentTypes) {
            if (_components.Count == 0) { return false; }

            return componentTypes.All(x => _components.ContainsKey(x));
        }

        public T Get<T>() where T : class {
            var type = typeof(T);
            if (_components.ContainsKey(type)) {
                return _components[typeof(T)] as T;
            } else {
                return null;
            }
        }

        public object Get(Type type) {
            if (_components.ContainsKey(type)) {
                return _components[type];
            } else {
                return null;
            }
        }

        public void Dispose() { RemoveAll(); }
    }
}