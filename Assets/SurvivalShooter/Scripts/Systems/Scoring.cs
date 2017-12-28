using UniRx;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter {
    public class Scoring : SystemBehaviour {
        public IntReactiveProperty Score { get; private set; } //component?

        public override void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory) {
            base.Initialize(eventSystem, poolManager, groupFactory);//-
            Score = new IntReactiveProperty();//-

            EventSystem.On<AxisInput, Died>((input, died) => {
                if (input == null) Score.Value++;
            }).AddTo(this);
        }
    }
}