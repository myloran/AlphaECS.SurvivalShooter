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
            types = new Type[] { typeof(T1) };
            var set = new Group<T1>(types, predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2> Create<T1, T2>() where T1 : class where T2 : class {
            types = new Type[] { typeof(T1), typeof(T2) };
            var set = new Group<T1, T2>(types, predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2, T3> Create<T1, T2, T3>() where T1 : class where T2 : class where T3 : class {
            types = new Type[] { typeof(T1), typeof(T2), typeof(T3) };
            var set = new Group<T1, T2, T3>(types, predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2, T3, T4> Create<T1, T2, T3, T4>() where T1 : class where T2 : class
            where T3 : class where T4 : class {
            types = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
            var set = new Group<T1, T2, T3, T4>(types, predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>()
            where T1 : class where T2 : class where T3 : class where T4 : class where T5 : class {
            types = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
            var set = new Group<T1, T2, T3, T4, T5>(types, predicates);
            Container.Inject(set);
            types = null;
            predicates.Clear();
            return set;
        }

        public Group<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>()
            where T1 : class where T2 : class where T3 : class where T4 : class
            where T5 : class where T6 : class {
            types = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)};
            var set = new Group<T1, T2, T3, T4, T5, T6>(types, predicates);
            Container.Inject(set);
            CleanUp();
            return set;
        }

        public Group<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>() 
            where T1 : class where T2 : class where T3 : class where T4 : class 
            where T5 : class where T6 : class where T7 : class {
            types = new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
            var set = new Group<T1, T2, T3, T4, T5, T6, T7>(types, predicates);
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