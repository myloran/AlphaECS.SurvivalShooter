﻿using UniRx;
using System;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter {
    [Serializable]
    public class MeleeAttack : ComponentBase {
        public BoolReactiveProperty TargetInRange = new BoolReactiveProperty();
        public IDisposable Attack;
        public IEntity Target;
        public int Damage;
        public float AttacksPerSecond;
    }
}