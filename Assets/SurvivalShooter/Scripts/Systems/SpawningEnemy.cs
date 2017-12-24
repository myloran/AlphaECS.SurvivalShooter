using UniRx;
using AlphaECS.Unity;
using Zenject;
using System;
using AlphaECS;

namespace AlphaECS.SurvivalShooter {
    public class SpawningEnemy : SystemBehaviour {
        // TODO remove this in favor of a factory...
        [Inject] DiContainer Container { get; set; }

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            GroupFactory.Create<Spawner>().OnAdd((enemySpawner, spawner) => {
                Observable.Timer(TimeSpan.FromSeconds(0f),
                    TimeSpan.FromSeconds(spawner.SpawnTime)).
                    Subscribe(_ => {
                        var enemy = PrefabFactory.Instantiate(spawner.Prefab, spawner.transform);
                        enemy.transform.position = spawner.transform.position;
                        enemy.transform.rotation = spawner.transform.rotation;
                    }).AddTo(spawner);
            }).AddTo(this);
        }
    }
}