using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlphaECS;
using System;
using AlphaECS.Unity;
using UniRx;

namespace AlphaECS.SurvivalShooter
{
	public class Deads : Group
	{
		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager)
		{
			Components = new Type[] { typeof(Health) };

			Func<IEntity, ReactiveProperty<bool>> checkIsDead = (e) =>
			{
				var health = e.Get<Health> ();
				health.CurrentHealth.Value = health.StartingHealth;

				var isDead = health.CurrentHealth.DistinctUntilChanged ().Select (value => value <= 0).ToReactiveProperty();
				return isDead;
			};

			Predicates.Add(checkIsDead);

			base.Initialize (eventSystem, poolManager);
		} 
	}
}
