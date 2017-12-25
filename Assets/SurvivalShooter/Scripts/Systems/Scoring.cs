using UniRx;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter {
    public class Scoring : SystemBehaviour {
        public IntReactiveProperty Score { get; private set; } //component?

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-
            Score = new IntReactiveProperty();//-

            GroupFactory.Create<InstantlyDied>().OnAdd((dead, _) => {
                if (dead.Has<AxisInput>()) return;

                Score.Value++;
                dead.Remove<InstantlyDied>();
            }).AddTo(this);

            //EventSystem.OnEvent<Died>().
            //    Where(died => !died.Target.Has<AxisInput>()).
            //    Subscribe(_ => Score.Value++).AddTo(this);
        }
    }
}