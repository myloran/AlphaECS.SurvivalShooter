using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using System;
using System.Linq;
using UnityEngine.AI;

namespace AlphaECS.SurvivalShooter {
    public class SpawningEnemyFX : SystemBehaviour {
        const float DeathSinkSpeed = 2.5f; //to settings

        public override void Initialize() {
            EventSystem.On<AxisInput, Health, View, Damaged>((input, health, view, damaged) => {
                if (input != null || health.Current.Value <= 0) return;

                view.Transforms[0].GetComponentsInChildren<AudioSource>().
                    Where(audio => audio.clip.name.Contains("Hurt")).
                    FirstOrDefault().Play();
                var particles = view.Transforms[0].GetComponentInChildren<ParticleSystem>();
                particles.transform.position = damaged.position;
                particles.Play();
            }).AddTo(this);

            GroupFactory.Create<View, Health, NavMeshAgent, CapsuleCollider, Animator, Rigidbody>().
                OnAdd((enemy, view, _____, ______, collider, animator, rigidbody) => {
                var transform = view.Transforms[0];//-

                enemy.Get<Health>().Current.DistinctUntilChanged().Where(value => value <= 0).Subscribe(_ => {
                    collider.isTrigger = true;
                    animator.SetTrigger("Die");
                    transform.GetComponentsInChildren<AudioSource>().
                        Where(audio => audio.clip.name.Contains("Death")).
                        FirstOrDefault().Play();
                    rigidbody.isKinematic = true;

                    Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(__ => {
                        var sink = Observable.EveryUpdate().Subscribe(___ => 
                            transform.Translate(-Vector3.up * DeathSinkSpeed * Time.deltaTime)).
                            AddTo(view.Disposer);
                        Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(____ => sink.Dispose());
                    });
                }).AddTo(view.Disposer);
            }).AddTo(this);
        }
    }
}