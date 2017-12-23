﻿using UnityEngine;
using System.Collections;
using Zenject;
using AlphaECS.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using AlphaECS;
using AlphaECS.Unity;
using DG.Tweening;
using System.Linq;
using System;

namespace AlphaECS.SurvivalShooter
{
	public class AnimatingPlayer : SystemBehaviour
	{
		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			var group = GroupFactory.Create (new Type[] { typeof(AxisInput), typeof(View), typeof(Animator) });

			group.OnAdd().Subscribe (entity =>
			{
				var input = entity.Get<AxisInput> ();
				var horizontal = input.Horizontal.DistinctUntilChanged ();
				var vertical = input.Vertical.DistinctUntilChanged ();
				var animator = entity.Get<Animator>();

				Observable.CombineLatest (horizontal, vertical, (h, v) =>
				{
					return h != 0f || v != 0f;
				}).ToReadOnlyReactiveProperty().DistinctUntilChanged().Subscribe(value =>
				{
					animator.SetBool("IsWalking", value);
				}).AddTo(input.Disposer);
			}).AddTo (Disposer);
		}
	}
}
