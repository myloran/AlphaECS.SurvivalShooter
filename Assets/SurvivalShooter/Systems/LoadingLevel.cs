using UniRx;
using UnityEngine;
using AlphaECS;
using AlphaECS.Unity;
using System;
using UniRx.Triggers;
using System.Collections;

namespace AlphaECS.SurvivalShooter {
    public class LoadingLevel : SystemBehaviour {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);

            EventSystem.OnEvent<Died>().Where(_ => _.Target.Has<AxisInput>()).Subscribe(_ => {
                Observable.EveryUpdate().Subscribe(__ => {
                    if (Input.GetMouseButton(0)) {
                        EventSystem.Publish(new LoadScene() { SceneName = "Level_01" });
                        Disposer.Clear();
                    }
                }).AddTo(Disposer).AddTo(this);
            }).AddTo(this);
        }
    }
}