using UnityEngine;
using AlphaECS;

namespace AlphaECS.SurvivalShooter
{
    public class Damaged
    {
		public IEntity Attacker { get; set; }
		public IEntity Target { get; set; }
        public int DamageAmount { get; private set; }
        public Vector3 HitPoint { get; private set; }

		public Damaged(IEntity attacker, IEntity target, int damageAmount, Vector3 hitPoint)
        {
			Attacker = attacker;
			Target = target;
			DamageAmount = damageAmount;
			HitPoint = hitPoint;
        }
    }
}