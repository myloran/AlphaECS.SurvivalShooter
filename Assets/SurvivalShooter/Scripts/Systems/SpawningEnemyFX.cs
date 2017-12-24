using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using System;
using System.Linq;
using UnityEngine.AI;

namespace AlphaECS.SurvivalShooter {
    public class SpawningEnemyFX : SystemBehaviour {
        const float DeathSinkSpeed = 2.5f; //to settings

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            EventSystem.OnEvent<Damaged>().
                Where(damaged => damaged.Target.Has<AxisInput>() == false).
                Subscribe(damaged => {
                    if (damaged.Target.Get<Health>().Current.Value <= 0) return;

                    var view = damaged.Target.Get<View>();//-
                    view.Transforms[0].GetComponentsInChildren<AudioSource>().
                        Where(audio => audio.clip.name.Contains("Hurt")).
                        FirstOrDefault().Play();

                    var particles = view.Transforms[0].GetComponentInChildren<ParticleSystem>();//-
                    particles.transform.position = damaged.Position;
                    particles.Play();
                }).AddTo(this);

            GroupFactory.Create<View, Health, NavMeshAgent, CapsuleCollider, Animator, Rigidbody>().
                OnAdd((enemy, view, health, navMeshAgent, collider, animator, rigidbody) => {
                var transform = view.Transforms[0];//-

                enemy.Get<Health>().Current.DistinctUntilChanged().Where(value => value <= 0).Subscribe(_ => {
                    collider.isTrigger = true;
                    animator.SetTrigger("Die");
                    transform.GetComponentsInChildren<AudioSource>().
                        Where(audio => audio.clip.name.Contains("Death")).
                        FirstOrDefault().Play();
                    rigidbody.isKinematic = true;

                    Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(__ => {
                        var sink = Observable.EveryUpdate().Subscribe(___ => {
                            transform.Translate(-Vector3.up * DeathSinkSpeed * Time.deltaTime);
                        }).AddTo(view.Disposer);

                        Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(____ => {
                            sink.Dispose();
                        });
                    });
                }).AddTo(view.Disposer);
            }).AddTo(this);
        }
    }
}