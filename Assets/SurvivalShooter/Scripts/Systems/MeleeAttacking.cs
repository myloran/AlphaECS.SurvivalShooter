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

            GroupFactory.Create<View, MeleeAttack>().OnAdd((attacker, view, attack) => {//what about aggregated components?
                attack.TargetInRange = new BoolReactiveProperty();//-

                SetTargetOnCollision(attack, view.Transforms[0].GetComponent<Collider>()); //extract collision system

                attack.TargetInRange.DistinctUntilChanged().Subscribe(targetInRange => {
                    if (targetInRange) {//make shortcut method for TimeSpan.FromSeconds()
                        attack.Attack = Observable.Timer(TimeSpan.FromSeconds(0f), 
                            TimeSpan.FromSeconds(1f / attack.AttacksPerSecond)).
                                Subscribe(_ => EventSystem.Publish(new Damaged(attack.Target, 
                                    attack.Damage, view.Transforms[0].position))).
                                AddTo(attack.Target.Get<View>().Disposer);
                    } else attack.Attack?.Dispose();
                }).AddTo(view.Disposer);
            }).AddTo(this);
        }

        void SetTargetOnCollision(MeleeAttack attack, Collider collider) {
            collider.OnTriggerEnterAsObservable().Subscribe(targetCollider => {
                var targetView = targetCollider.GetComponent<EntityBehaviour>();//name it properly etc
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