using UnityEngine;
using System.Collections;
using Zenject;
using AlphaECS.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using AlphaECS;
using AlphaECS.Unity;
using UniRx.Triggers;
using System;

public class GameOverScreen : ComponentBehaviour
{
	public Animator animator;

	public override void Initialize (IEventSystem eventSystem)
	{
		base.Initialize (eventSystem);

		EventSystem.OnEvent<Died> ().Where (_ => _.Target.Has<AxisInput> ()).Subscribe (_ =>
		{
			animator.SetTrigger("GameOver");
            Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe(__ => this.OnMouseDownAsObservable().Subscribe(x => Debug.Log("restarting level")));
            
		}).AddTo (this);
	}
}
