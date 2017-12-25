using UnityEngine;

namespace AlphaECS.SurvivalShooter {
    public class Damaged /*: BaseEvent<IEntity, IEntity, int, Vector3>*/ {
        public IEntity Attacker { get; set; }//replace it with IEntity<View, MeleeAtack>
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