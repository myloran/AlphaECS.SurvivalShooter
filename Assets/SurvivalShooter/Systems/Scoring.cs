using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using AlphaECS;
using System;

namespace AlphaECS.SurvivalShooter
{
	public class Scoring : SystemBehaviour
	{
		public IntReactiveProperty Score { get; private set; }

		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			Score = new IntReactiveProperty ();

			EventSystem.OnEvent<Died> ().Where (_ => !_.Target.Has<AxisInput> ()).Subscribe (_ =>
			{
				Score.Value++;
			}).AddTo (this);
		}
	}
}
