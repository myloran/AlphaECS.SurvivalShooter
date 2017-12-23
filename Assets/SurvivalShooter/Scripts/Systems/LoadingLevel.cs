using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using System;

namespace AlphaECS.SurvivalShooter {
    public class LoadingLevel : SystemBehaviour {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            EventSystem.OnEvent<Died>().Where(died => died.Target.Has<AxisInput>()).Subscribe(_ => {
                Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe(__ => {
                    Observable.EveryUpdate().Subscribe(___ => {
                        if (Input.GetMouseButton(0)) {
                            EventSystem.Publish(new LoadScene() { Name = "Level_01" });
                            Disposer.Clear();
                        }
                    }).AddTo(Disposer).AddTo(this);//what is this even mean?)
                }).AddTo(this);
            }).AddTo(this);
        }
    }
}