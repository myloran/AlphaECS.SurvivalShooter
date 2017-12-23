using UnityEngine;

namespace AlphaECS.SurvivalShooter {
    public class Damaged {
        public IEntity Attacker { get; set; }
        public IEntity Target { get; set; }
        public int DamageAmount { get; private set; }
        public Vector3 Position { get; private set; }

        public Damaged(IEntity attacker, IEntity target, int damageAmount, Vector3 position) {//-
            Attacker = attacker;//-
            Target = target;//-
            DamageAmount = damageAmount;//-
            Position = position;//-
        }//-
    }
}