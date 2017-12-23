using AlphaECS.SurvivalShooter;
using UnityEngine;
using AlphaECS;

[CreateAssetMenu(menuName = "Blueprint/MeleeEnemy")]
public class MeleeEnemyBlueprint : BlueprintBase {
    public Health health = new Health();
    public Melee melee = new Melee();

    public override void Apply(IEntity entity) {
        base.Apply(entity); //-

        var clone = Instantiate(this);
        entity.Add(clone.health);
        entity.Add(clone.melee);
    }
}