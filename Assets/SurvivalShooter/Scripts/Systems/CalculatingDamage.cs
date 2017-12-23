using UniRx;
using AlphaECS.Unity;
using Zenject;
using System;

namespace AlphaECS.SurvivalShooter {
    public class CalculatingDamage : SystemBehaviour {
        [Inject]
        public Deads deads { get; set; }

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            deads.OnAdd().Subscribe(dead => {
                var view = dead.Get<View>();//-
                Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => {
                    PoolManager.GetPool().RemoveEntity(dead);//combine them as default
                    Destroy(view.Transforms[0].gameObject);
                }).AddTo(view.Disposer);
            }).AddTo(Disposer);

            EventSystem.OnEvent<Damaged>().Subscribe(damaged => {
                var health = damaged.Target.Get<Health>();//-
                if (health.Current.Value <= 0) return;

                health.Current.Value -= damaged.DamageAmount;
                if (health.Current.Value <= 0) EventSystem.Publish(new Died(damaged.Attacker, damaged.Target));
            }).AddTo(this);
        }
    }
}