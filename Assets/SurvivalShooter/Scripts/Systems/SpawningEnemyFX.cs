using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.AI;

namespace AlphaECS.SurvivalShooter
{
	public class SpawningEnemyFX : SystemBehaviour
	{
		const float DeathSinkSpeed = 2.5f;

		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			EventSystem.OnEvent<Damaged> ().Where(_ => _.Target.Has<AxisInput>() == false).Subscribe (_ =>
			{
				if(_.Target.Get<Health>().Current.Value <= 0)
				{ return; }

				var viewComponent = _.Target.Get<View>();
				var soundFX = viewComponent.Transforms[0].GetComponentsInChildren<AudioSource>();
				var hurtFX = soundFX.Where(_2 => _2.clip.name.Contains("Hurt")).FirstOrDefault();
				hurtFX.Play();

				var particles = viewComponent.Transforms[0].GetComponentInChildren <ParticleSystem> ();
				particles.transform.position = _.HitPoint;
				particles.Play();
			}).AddTo (this);

			var group = GroupFactory.Create (new Type[]{ typeof(View), typeof(Health), typeof(NavMeshAgent), typeof(CapsuleCollider), typeof(Animator), typeof(Rigidbody) });
			group.OnAdd().Subscribe (entity =>
			{
				var viewComponent = entity.Get<View> ();
				var targetTransform = viewComponent.Transforms[0];
				var healthComponent = entity.Get<Health> ();
				var capsuleCollider = entity.Get<CapsuleCollider>();
				var animator = entity.Get<Animator>();
				var rb = entity.Get<Rigidbody>();

				healthComponent.Current.DistinctUntilChanged ().Where (value => value <= 0).Subscribe (_ =>
				{
					capsuleCollider.isTrigger = true;
					animator.SetTrigger ("Die");
					var soundFX = targetTransform.GetComponentsInChildren<AudioSource> ();
					var deathFX = soundFX.Where(_2 => _2.clip.name.Contains("Death")).FirstOrDefault();
					deathFX.Play();

					rb.isKinematic = true;
//					ScoreManager.score += scoreValue;

					Observable.Timer (TimeSpan.FromSeconds (1)).Subscribe (_2 =>
					{
						var sink = Observable.EveryUpdate ().Subscribe (_3 =>
						{
							targetTransform.Translate (-Vector3.up * DeathSinkSpeed * Time.deltaTime);
						}).AddTo(viewComponent.Disposer);

						Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_3 =>
						{
							sink.Dispose ();
						});
					});
				}).AddTo (viewComponent.Disposer);
			}).AddTo (this);
		}
	}
}
