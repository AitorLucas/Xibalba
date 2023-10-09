using UnityEngine;

[CreateAssetMenu()]
public class ImprovementSO : ScriptableObject {

    public string title;
    public string description;
    public ImprovementType improvementType;
    public ImprovementRarity improvementRarity;

}

public enum ImprovementType {
    MovementVelocityUp5PerCent,
    MaxLifeUp10PerCent,
    FireRateUp10PerCent,
    SlowDownEnemy5PerCentFor3Seconds,
    Heal5PerCentMaxLifeOnKill,
    IncreaseExplosionSpellRange10PerCent,
    ExtraShot,
    ExtraDamage,
    PiercingShot,
    ExplosionSpellFireTrail,
    LaserSpellGlobalRange,
    ChoosePositionOnExplosionSpell,
    Shield20PerCentMaxLife,
}

public enum ImprovementRarity: int {
    Bronze = 15,
    Silver = 5,
    Gold = 1,
}
