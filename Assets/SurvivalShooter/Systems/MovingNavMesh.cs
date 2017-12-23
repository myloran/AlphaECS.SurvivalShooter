using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;
using AlphaECS;
using UnityEngine.AI;

namespace AlphaECS.SurvivalShooter
{
	public class MovingNavMesh : SystemBehaviour
	{
		Transform Target;
		Health TargetHealth;

		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			var group = GroupFactory.Create (new Type[] { typeof(Health), typeof(View), typeof(UnityEngine.AI.NavMeshAgent) });
			group.OnAdd().Subscribe (entity =>
			{
				var viewComponent = entity.Get<View>();
				var navMeshAgent = entity.Get<NavMeshAgent> ();
				var health = entity.Get<Health> ();

				Observable.EveryUpdate ().Subscribe (_ =>
				{
					if (Target == null)
					{
						var go = GameObject.FindGameObjectWithTag ("Player");
						if (go == null)
						{ return; }

						Target = go.transform;
						if (Target == null)
						{ return; }

						TargetHealth = Target.GetComponent<EntityBehaviour> ().Entity.Get<Health> ();
						if (TargetHealth == null)
						{ return; }
					}

					if (health.CurrentHealth.Value > 0 && TargetHealth.CurrentHealth.Value > 0)
					{
						navMeshAgent.SetDestination (Target.position);
					} else
					{
						navMeshAgent.enabled = false;
					}
				}).AddTo (navMeshAgent).AddTo(health);
			}).AddTo (this);
		}
	}
}
