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

            GroupFactory.Create<Health, TookDamage>().OnAdd((entity, health, tookDamage) => {
                if (health.Current.Value <= 0) return;

                health.Current.Value -= tookDamage.amount;
                if (health.Current.Value <= 0) entity.Add(new InstantlyDied());
                entity.Remove<TookDamage>();
                entity.Add(new SpawnParticles() { position = tookDamage.position });
            }).AddTo(this);

            //EventSystem.OnEvent<Damaged>().Subscribe(damaged => {
            //    var health = damaged.Target.Get<Health>();//what if target does not have Health?
            //    if (health.Current.Value <= 0) return;

            //    health.Current.Value -= damaged.DamageAmount;
            //    if (health.Current.Value <= 0) EventSystem.Publish(new Died(damaged.Attacker, damaged.Target));
            //}).AddTo(this);
        }
    }
}
