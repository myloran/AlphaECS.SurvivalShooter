using System;
using UniRx;

namespace AlphaECS.SurvivalShooter {
    public class Deads : Group<Health> {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager) {
            AddPredicate((entity, health) => {
                health.Current.Value = health.Starting;
                return health.Current.DistinctUntilChanged().Select(value => value <= 0).ToReactiveProperty();
            });
            base.Initialize(eventSystem, poolManager);
        }
    }
}