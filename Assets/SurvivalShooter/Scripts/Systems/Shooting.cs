using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using System;

namespace AlphaECS.SurvivalShooter {
    public class Shooting : SystemBehaviour {
        Ray ray;//-
        RaycastHit hit;//-
        int mask;//-

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            mask = LayerMask.GetMask("Shootable");
            var shooters = GroupFactory.Create(new Type[] { typeof(View), typeof(Shooter) });
            shooters.OnAdd().Subscribe(player => {
                var view = player.Get<View>();//-
                var transform = view.Transforms[0];//-
                var shooter = player.Get<Shooter>();//-
                shooter.IsShooting = new BoolReactiveProperty();

                var gun = transform.Find("GunBarrelEnd");//there must be a better solution??? 
                var particle = gun.GetComponent<ParticleSystem>();//-
                var line = gun.GetComponent<LineRenderer>();//-
                var audio = gun.GetComponent<AudioSource>();//-
                var light = gun.GetComponent<Light>();//-

                shooter.IsShooting.DistinctUntilChanged().Subscribe(isShooting => {
                    if (isShooting) {
                        shooter.Shoot = Observable.Timer(TimeSpan.FromSeconds(0f), 
                            TimeSpan.FromSeconds(1f / shooter.ShotsPerSecond)).
                            Subscribe(_ => {
                                ray.origin = gun.position;
                                ray.direction = gun.forward;

                                if (Physics.Raycast(ray, out hit, shooter.Range, mask)) {
                                    var targetView = hit.collider.GetComponent<EntityBehaviour>();
                                    if (targetView?.Entity.Get<Health>() != null) {
                                        EventSystem.Publish(new Damaged(player, targetView.Entity, shooter.Damage, hit.point));
                                    }
                                    line.SetPosition(1, hit.point);
                                } else line.SetPosition(1, ray.origin + ray.direction * shooter.Range);

                                audio.Play();//separate fx system
                                light.enabled = true;
                                particle.Stop();
                                particle.Play();
                                line.enabled = true;
                                line.SetPosition(0, gun.position);

                                Observable.Timer(TimeSpan.FromSeconds((1f / shooter.ShotsPerSecond) / 2f)).Subscribe(__ => {
                                    line.enabled = false;
                                    light.enabled = false;
                                }).AddTo(Disposer).AddTo(view.Disposer);
                        }).AddTo(Disposer).AddTo(shooter.Disposer);
                    } else shooter.Shoot?.Dispose();
                }).AddTo(Disposer).AddTo(shooter.Disposer);
            }).AddTo(Disposer);

            Observable.EveryUpdate().Subscribe(_ => {//input system
                foreach (var player in shooters.Entities) {
                    var shooter = player.Get<Shooter>();//-
                    if (Input.GetButton("Fire1")) shooter.IsShooting.Value = true;
                    else shooter.IsShooting.Value = false;
                }
            }).AddTo(Disposer);
        }
    }
}