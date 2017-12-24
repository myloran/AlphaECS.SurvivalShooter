using UnityEngine;
using AlphaECS.Unity;
using UniRx;
using Zenject;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace AlphaECS.SurvivalShooter {
    public class SpawningPlayerFX : SystemBehaviour {
        public Slider HealthSlider;//-
        public Image DamageImage;//-
        public float FlashSpeed = 5f;//-
        public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);//-

        [Inject]
        public Deads deads { get; set; }

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-

            GroupFactory.Create<View, Health, AxisInput, Animator>().OnAdd((player, view, health, _, __) => {
                var previousHealth = health.Current.Value;

                health.Current.DistinctUntilChanged().Subscribe(currentHealth => {
                    if (currentHealth < previousHealth && currentHealth >= 0) {
                        if (DamageImage != null) {
                            DamageImage.color = FlashColor;
                            DOTween.To(() => DamageImage.color, x => DamageImage.color = x, Color.clear, FlashSpeed);
                        }
                        view.Transforms[0].GetComponentsInChildren<AudioSource>().
                            Where(audioSource => audioSource.clip.name.Contains("Hurt")).
                            FirstOrDefault().Play();
                    }
                    HealthSlider.value = currentHealth;
                    previousHealth = currentHealth;
                }).AddTo(Disposer).AddTo(health.Disposer);
            }).AddTo(Disposer);

            deads.OnAdd().Where(dead => dead.Has<AxisInput>()).Subscribe(dead => {
                dead.Get<Animator>().SetTrigger("Die");
                dead.Get<View>().Transforms[0].GetComponentsInChildren<AudioSource>().
                    Where(audioSource => audioSource.clip.name.Contains("Death")).
                    FirstOrDefault().Play();
            }).AddTo(Disposer);
        }
    }
}