using AlphaECS.Unity;
using UniRx;
using System;

namespace AlphaECS.SurvivalShooter {
    [Serializable]
    public class Health : ComponentBase {
        public IntReactiveProperty Current = new IntReactiveProperty();
        public int Starting;
        public bool IsDamaged;
    }
}