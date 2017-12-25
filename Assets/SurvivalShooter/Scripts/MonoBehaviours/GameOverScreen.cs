using UnityEngine;
using AlphaECS.SurvivalShooter;
using UniRx;
using AlphaECS;
using AlphaECS.Unity;
using UniRx.Triggers;

public class GameOverScreen : ComponentBehaviour {
    public Animator animator;

    public override void Initialize(IEventSystem eventSystem) {
        base.Initialize(eventSystem);//-

        //EventSystem.OnEvent<Died>().Where(died => died.Target.Has<AxisInput>()).Subscribe(_ => {
        //    animator.SetTrigger("GameOver");
        //    this.OnMouseDownAsObservable().Subscribe(x => Debug.Log("restarting level"));
        //}).AddTo(this);
    }
}