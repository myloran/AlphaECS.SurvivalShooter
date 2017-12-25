using AlphaECS.Unity;
using UniRx;
using UnityEngine;

namespace AlphaECS.SurvivalShooter {
    public class Deads : Group<Health, View, Animator> {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager) {
            AddPredicate((entity, health, _, __) => {
                health.Current.Value = health.Starting;
                return health.Current.DistinctUntilChanged().Select(value => value <= 0).ToReactiveProperty();
            });
            base.Initialize(eventSystem, poolManager);
        }
    }
}