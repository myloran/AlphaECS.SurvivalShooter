using UnityEngine;
using UniRx;
using AlphaECS.Unity;
using System;

namespace AlphaECS.SurvivalShooter {
    public class AnimatingPlayer : SystemBehaviour {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            GroupFactory.Create<AxisInput, View, Animator>().OnAdd((player, input, view, animator) => {
                Observable.CombineLatest(input.Horizontal.DistinctUntilChanged(), input.Vertical.DistinctUntilChanged(), 
                    (horizontal, vertical) => horizontal != 0f || vertical != 0f).
                    ToReadOnlyReactiveProperty().DistinctUntilChanged().// is this really needed?
                    Subscribe(isWalking => animator.SetBool("IsWalking", isWalking)).AddTo(input.Disposer);
            }).AddTo(Disposer);
        }
    }
}