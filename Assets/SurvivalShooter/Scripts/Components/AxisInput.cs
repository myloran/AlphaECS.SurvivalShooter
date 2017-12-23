using UniRx;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter {
    public class AxisInput : ComponentBase {
        public FloatReactiveProperty Horizontal = new FloatReactiveProperty();
        public FloatReactiveProperty Vertical = new FloatReactiveProperty();
    }
}