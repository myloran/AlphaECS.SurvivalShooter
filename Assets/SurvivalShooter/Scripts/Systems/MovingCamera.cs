using UnityEngine;
using AlphaECS.Unity;
using System;
using UniRx;

namespace AlphaECS.SurvivalShooter {
    public class MovingCamera : SystemBehaviour {
        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            var group = GroupFactory.Create(new Type[] { typeof(Camera), typeof(Follower) });
            group.OnAdd().Subscribe(camera => {
                var follower = camera.Get<Follower>();//-
                follower.Offset = follower.transform.position - follower.Target.position;

                Observable.EveryFixedUpdate().Subscribe(_ => {
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