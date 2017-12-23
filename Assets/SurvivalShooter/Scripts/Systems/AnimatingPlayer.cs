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

                Observable.CombineLatest(input.Horizontal.DistinctUntilChanged(), input.Vertical.DistinctUntilChanged(), 
                    (horizontal, vertical) => horizontal != 0f || vertical != 0f).//-
                    ToReadOnlyReactiveProperty().DistinctUntilChanged().
                    Subscribe(value => player.Get<Animator>().SetBool("IsWalking", value)).AddTo(input.Disposer);
            }).AddTo(Disposer);
        }
    }
}