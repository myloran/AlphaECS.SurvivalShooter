using UniRx;
using UnityEngine;
using AlphaECS;
using AlphaECS.Unity;
using UniRx.Triggers;
using System;

namespace AlphaECS.SurvivalShooter {    
    public class MeleeAttacking : SystemBehaviour {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            var group = GroupFactory.Create(new Type[] { typeof(View), typeof(MeleeAttack) });
            group.OnAdd().Subscribe(attacker => {
                var view = attacker.Get<View>();//-
                var attack = attacker.Get<MeleeAttack>();//-
                attack.TargetInRange = new BoolReactiveProperty();//-

                SetTargetOnCollision(attack, view.Transforms[0].GetComponent<Collider>()); //extract collision system

                attack.TargetInRange.DistinctUntilChanged().Subscribe(targetInRange => {
                    if (targetInRange) {
                        attack.Attack = Observable.Timer(TimeSpan.FromSeconds(0f),
                            TimeSpan.FromSeconds(1f / attack.AttacksPerSecond)).
                            Subscribe(_ => {//make shortcut method for TimeSpan.FromSeconds()
                                var attackPosition = attack.Target.Get<View>().Transforms[0].position;//-
                                EventSystem.Publish(new Damaged(attacker, attack.Target, attack.Damage, attackPosition));
                            }).AddTo(attack.Target.Get<View>().Disposer);
                    } else attack.Attack?.Dispose();
                }).AddTo(view.Disposer);
            }).AddTo(this);
        }

        void SetTargetOnCollision(MeleeAttack attack, Collider collider) {
            collider.OnTriggerEnterAsObservable().Subscribe(targetCollider => {
                var targetView = targetCollider.GetComponent<EntityBehaviour>();//=
                if (targetView == null || !targetView.Entity.Has<AxisInput>() || !targetView.Entity.Has<Health>()) return;

                attack.Target = targetView.Entity;
                attack.TargetInRange.Value = true;

            }).AddTo(collider);

            collider.OnTriggerExitAsObservable().Subscribe(targetCollider => {
                var targetView = targetCollider.GetComponent<EntityBehaviour>();//-
                if (targetView == null || !targetView.Entity.Has<AxisInput>() || !targetView.Entity.Has<Health>()) return;

                attack.Target = null;
                attack.TargetInRange.Value = false;
            }).AddTo(collider);
        }
    }
}