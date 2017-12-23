using UnityEngine;
using UniRx;
using AlphaECS.Unity;
using System;

namespace AlphaECS.SurvivalShooter {
    public class AnimatingPlayer : SystemBehaviour {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            var group = GroupFactory.Create(new Type[] { typeof(AxisInput), typeof(View), typeof(Animator) });
            group.OnAdd().Subscribe(player => {
                var input = player.Get<AxisInput>();//-
                var horizontal = input.Horizontal.DistinctUntilChanged();//-
                var vertical = input.Vertical.DistinctUntilChanged();//-
                var animator = player.Get<Animator>();//-

                Observable.CombineLatest(horizontal, vertical, (h, v) => h != 0f || v != 0f).//-
                    ToReadOnlyReactiveProperty().DistinctUntilChanged().
                    Subscribe(value => animator.SetBool("IsWalking", value)).AddTo(input.Disposer);
            }).AddTo(Disposer);
        }
    }
}