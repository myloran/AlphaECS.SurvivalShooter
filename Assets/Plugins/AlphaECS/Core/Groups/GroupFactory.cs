using System;
using System.Collections.Generic;
using AlphaECS;
using Zenject;
using UniRx;
using System.Linq;

namespace AlphaECS
{
//	public class GroupFactory : Factory<Type[], Group>
	public class GroupFactory
    {
		[Inject] protected DiContainer Container = null;
//		protected Dictionary<HashSet<Type>, Group> Groups = new Dictionary<HashSet<Type>, Group>();

		private Type[] types;
		private List<Func<IEntity, ReactiveProperty<bool>>> predicates = new List<Func<IEntity, ReactiveProperty<bool>>> ();

		public Group Create(Type[] _types)
		{
			this.types = _types;
			return this.Create ();
		}

        public Group<T1> Create<T1>() where T1 : class {
            var set = new Group<T1>(predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2> Create<T1, T2>() where T1 : class where T2 : class {
            var set = new Group<T1, T2>(predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2, T3> Create<T1, T2, T3>() where T1 : class where T2 : class where T3 : class {
            var set = new Group<T1, T2, T3>(predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2, T3, T4> Create<T1, T2, T3, T4>() where T1 : class where T2 : class
            where T3 : class where T4 : class {
            var set = new Group<T1, T2, T3, T4>(predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>()
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class {
            var set = new Group<T1, T2, T3, T4, T5>(predicates);
            Container.Inject(set);
            types = null;
            predicates.Clear();
            return set;
        }

        public Group<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>()
            where T1 : class where T2 : class where T3 : class where T4 : class
            where T5 : class where T6 : class {
            var set = new Group<T1, T2, T3, T4, T5, T6>(predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>() 
            where T1 : class where T2 : class where T3 : class where T4 : class 
            where T5 : class where T6 : class where T7 : class {
            var set = new Group<T1, T2, T3, T4, T5, T6, T7>(predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        void CleanUp() {
            types = null;
            predicates.Clear();
        }

        public Group Create()
		{
//			var hashSet = new HashSet<Type> (types);
//			foreach (var key in Groups.Keys)
//			{
//				if (hashSet.SetEquals(key) && predicates.Count == 0)
//				{
//					this.types = null;
//					this.predicates.Clear();
//					return Groups [key];
//				}
//			}
				
			var group = new Group (types, predicates);
			Container.Inject (group);

			types = null;
			predicates.Clear();
//			Groups.Add (hashSet, group);
			return group;
		}

		public GroupFactory AddTypes(Type[] _types)
		{
			this.types = _types;
			return this;
		}

		public GroupFactory WithPredicate(Func<IEntity, ReactiveProperty<bool>> predicate)
		{
			this.predicates.Add (predicate);
			return this;
		}
    }
}