using UnityEngine;

namespace AlphaECS.SurvivalShooter {
    public class Damaged : IEvent {
        public IEntity entity { get; set; }
        public int amount { get; private set; }
        public Vector3 position { get; private set; }

        public Damaged(IEntity target, int amount, Vector3 position) {
            this.entity = target;
            this.amount = amount;
            this.position = position;
        }
    }
}