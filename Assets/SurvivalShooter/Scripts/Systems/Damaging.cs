using UniRx;
using AlphaECS.Unity;
using Zenject;
using System;

namespace AlphaECS.SurvivalShooter {
    public class Damaging : SystemBehaviour {
        [Inject]
        public Deads deads { get; set; }

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            deads.OnAdd((dead, _, view, __) => {
                Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(___ => {
                    PoolManager.GetPool().RemoveEntity(dead);//combine them as default
                    Destroy(view.Transforms[0].gameObject);
                }).AddTo(view.Disposer);
            }).AddTo(Disposer);

            EventSystem.On<Health, View, Damaged>((health, view, damaged) => {
                if (health.Current.Value <= 0) return;

                health.Current.Value -= damaged.amount;
                if (health.Current.Value <= 0) EventSystem.Publish(new Died(damaged.entity));
            }).AddTo(this);
        }
    }
}
