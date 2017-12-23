using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;

namespace AlphaECS.SurvivalShooter
{
	public class CalculatingDamage : SystemBehaviour 
	{
		[Inject]
		public Deads deads { get; set; }

		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			deads.OnAdd ().Subscribe (entity =>
			{
				var viewComponent = entity.Get<View>();
				Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_2 =>
				{
					PoolManager.GetPool ().RemoveEntity (entity);
					GameObject.Destroy (viewComponent.Transforms[0].gameObject);
				}).AddTo(viewComponent.Disposer);
			}).AddTo (this.Disposer);

			var group = GroupFactory.AddTypes (new Type[] { typeof(View), typeof(Health) }).Create ();
			group.AddTo (this.Disposer);
				
			EventSystem.OnEvent<Damaged> ().Subscribe (_ =>
			{
				var targetHealth = _.Target.Get<Health>();
				if(targetHealth.CurrentHealth.Value <= 0)
					return;

				targetHealth.CurrentHealth.Value -= _.DamageAmount;

				if(targetHealth.CurrentHealth.Value <= 0)
				{
					EventSystem.Publish (new Died (_.Attacker, _.Target));
				}
			}).AddTo (this);
		}
	}
}
