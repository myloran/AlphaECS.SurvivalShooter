using UnityEngine;
using AlphaECS.Unity;
using System;
using UniRx;

namespace AlphaECS.SurvivalShooter {
    public class MovingCamera : SystemBehaviour {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            GroupFactory.Create<Camera, Follower>().OnAdd((_, __, follower) => {
                follower.Offset = follower.transform.position - follower.Target.position;

                Observable.EveryFixedUpdate().Subscribe(___ => {
                    if (follower.Target == null)
                        return;

                    Vector3 targetCamPos = follower.Target.position + follower.Offset;
                    follower.transform.position = Vector3.Lerp(follower.transform.position, 
                        targetCamPos, follower.Smoothing * Time.deltaTime);
                }).AddTo(follower);
            }).AddTo(this);
        }
    }
}