﻿using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using EcsRx.Groups;
using UniRx;
using Zenject;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

namespace EcsRx.SurvivalShooter
{
	public class PlayerFXSystem : ReactiveSystemBehaviour
	{
		public Slider HealthSlider;
		public Image DamageImage;
		public float FlashSpeed = 5f;
		public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);

		[Inject]
		DiContainer Container = null;

		void Awake()
		{
		}

		void Start()
		{
		}
			
		public override void Setup ()
		{
			base.Setup ();

			var group = new ReactiveGroup (typeof(ViewComponent), typeof(HealthComponent), typeof(InputComponent));

			group.Entities.ObserveAdd ().Subscribe (_ =>
			{
				var health = _.Value.GetComponent<HealthComponent> ();
				var previousValue = health.CurrentHealth.Value;

				var audioSources = _.Value.GetComponent<ViewComponent> ().View.GetComponentsInChildren<AudioSource>();

				var hurtSound = audioSources.Where(audioSource => audioSource.clip.name.Contains("Hurt")).FirstOrDefault();
				var deathSound = audioSources.Where(audioSource => audioSource.clip.name.Contains("Death")).FirstOrDefault();

				var animator = _.Value.GetComponent<ViewComponent> ().View.GetComponent<Animator>();

				health.CurrentHealth.DistinctUntilChanged ().Subscribe (value =>
				{
					if (value >= previousValue)
						return;

					if(value > 0)
					{
						DamageImage.color = FlashColor;
						DOTween.To (() => DamageImage.color, x => DamageImage.color = x, Color.clear, FlashSpeed);
	
						HealthSlider.value = value;
						hurtSound.Play();				
					}
					else
					{
						animator.SetTrigger ("Die");
						deathSound.Play();	
					}
				}).AddTo (this).AddTo (gameObject);
			}).AddTo(this).AddTo(group);

			Container.Inject (group);
		}

		public override System.Collections.IEnumerator SetupAsync ()
		{
			return base.SetupAsync ();
		}
	}
}
