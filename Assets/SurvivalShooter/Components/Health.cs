using AlphaECS.Unity;
using UniRx;
using System;

namespace AlphaECS.SurvivalShooter {
    [Serializable]
    public class Health : ComponentBase {
        public IntReactiveProperty CurrentHealth = new IntReactiveProperty();
        public int StartingHealth;
        public bool IsDamaged;
    }
}