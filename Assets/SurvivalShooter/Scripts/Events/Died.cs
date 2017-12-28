using UnityEngine;

namespace AlphaECS.SurvivalShooter {
    public class Died : IEvent{
        public IEntity entity { get; set; }

        public Died(IEntity entity) {
            this.entity = entity;
        }
    }
}