using UnityEngine;

[CreateAssetMenu()]
public class ImprovementSO : ScriptableObject {

    public string title;
    public string description;
    public ImprovementType improvementType;
    public ImprovementRarity improvementRarity;
    public Sprite sprite;

}

public enum ImprovementType {
    MovementVelocityUp,
    MaxLifeUp,
    FireRateUp,
    SlowDownEnemyOnHit,
    HealOnKill,
    IncreaseExplosionSpellRange,
    ExtraShot,
    ExtraDamage,
    PiercingShot,
    ExplosionSpellFireTrail,
    LaserSpellGlobalRange,
    ChoosePositionOnExplosionSpell,
    Shield,
    Ascension,
}

public enum ImprovementRarity: int {
    Bronze = 5,
    Silver = 3,
    Gold = 1,
}
