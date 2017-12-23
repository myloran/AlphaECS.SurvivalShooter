using UnityEngine;
using AlphaECS;

namespace AlphaECS.SurvivalShooter
{
    public class Died
    {
		public IEntity Attacker { get; set; }
		public IEntity Target { get; set; }

		public Died(IEntity attacker, IEntity target)
        {
			Attacker = attacker;
			Target = target;
        }
    }
}