using UniRx;
using System;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter {
    public class Shooter : ComponentBase {
        public BoolReactiveProperty IsShooting = new BoolReactiveProperty();
        public IDisposable Shoot;
        public int Damage;
        public float Range;
        public float ShotsPerSecond;
    }
}