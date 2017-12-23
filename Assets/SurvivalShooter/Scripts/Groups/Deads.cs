using System;
using UniRx;

namespace AlphaECS.SurvivalShooter {
    public class Deads : Group {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager) {
            Components = new Type[] { typeof(Health) };
            Predicates.Add((entity) => {
                var health = entity.Get<Health>();
                health.Current.Value = health.Starting;//-
                return health.Current.DistinctUntilChanged().Select(value => value <= 0).ToReactiveProperty();
            });
            base.Initialize(eventSystem, poolManager);
        }
    }
}