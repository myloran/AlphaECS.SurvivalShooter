using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;
using AlphaECS;

namespace AlphaECS.SurvivalShooter
{
	public class SpawningEnemy : SystemBehaviour
	{
		// TODO remove this in favor of a factory...
		[Inject] DiContainer Container { get; set; }

		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			var group = GroupFactory.Create (new Type[]{ typeof(Spawner) });
			group.OnAdd().Subscribe (entity =>
			{
				var spawner = entity.Get<Spawner>();
				var delay = TimeSpan.FromSeconds(0f);
				var interval = TimeSpan.FromSeconds(spawner.SpawnTime);
				Observable.Timer(delay, interval).Subscribe(_ =>
				{
//			        if(playerHealth.currentHealth <= 0f)
//			        {
//			            return;
//			        }

//					Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

					var instance = PrefabFactory.Instantiate (spawner.Prefab, spawner.transform);
					// instance.transform.SetParent(spawner.transform, false);
					instance.transform.position = spawner.transform.position;
					instance.transform.rotation = spawner.transform.rotation;
				}).AddTo(spawner);
			}).AddTo (this);
		}
	}
}
