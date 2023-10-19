using System;
using System.Collections;
using System.Reflection.Emit;
using UnityEngine;
using static ImprovementManager;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : ISingleton<Player> {

    [Range(50, 300f)][SerializeField] private float maxLifes = 100;
    [Range(0, 2f)][SerializeField] private float invecibilityDelay = 0.8f;
    [Header("Movement")]
    [Range(3, 30f)][SerializeField] private float movementSpeed = 6f;
    [Range(0, .5f)][SerializeField] private float movementSmoothing = .3f;
    [Header("Shoot")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private ProjectileSO shotProjectileSO;
    [SerializeField] private ProjectileSO laserProjectileSO;
    [SerializeField] private ProjectileSO breathProjectileSO;
    [SerializeField] private ProjectileSO explosionProjectileSO;
    [Range(5, 60f)][SerializeField] private float shotSpeed = 25f;

    public event EventHandler<OnPlayerLifeChangedArgs> OnPlayerLifeChanged;
    public class OnPlayerLifeChangedArgs : EventArgs {
        public float currentLifeNormalized;
    }
    public event EventHandler<OnExplosionPositionFreeChangedArgs> OnExplosionPositionFreeChanged;
    public class OnExplosionPositionFreeChangedArgs : EventArgs {
        public bool isActive;
    }
    public event EventHandler<OnLaserGlobalRangeChangedArgs> OnLaserGlobalRangeChanged;
    public class OnLaserGlobalRangeChangedArgs : EventArgs {
        public bool isActive;
    }
    public event EventHandler<OnExplosionRangeMultiplierChangedArgs> OnExplosionRangeMultiplierChanged;
    public class OnExplosionRangeMultiplierChangedArgs : EventArgs {
        public float multiplier;
    }


    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    private bool isGodModeEnable = false;
    private bool isInvecible = false;
    private float shieldLife = 0;
    private float lifes;

    // - Improvements
    private float movementVelocityMultiplier = 1f;
    private float maxLifesMultiplier = 1f;
    private float shotSpeedMultiplier = 1f;
    private float slowEnemyMultiplier = 1f;
    private float recoverLifeMultiplierOnKill = 1f;
    private float explosionRangeMultiplier = 1f;
    private int extrasShots = 0;
    private float damagesMultiplier = 1f;
    private bool isPiercingShots = false; // TODO: CHANGE TO INT
    private bool isExplosionWithFireTrails = false; // TODO: NOT IMPLEMENTED
    private bool isLaserWithGlobalRange = false;
    private bool isExplosionPositionFree = false;
    private bool isShieldActivatable = false; // TODO: NEEDS VISUAL

    protected override void Awake() {
        base.Awake();

        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        lifes = maxLifes;
    }

    private void Start() {
        ConstructPlayer();
    
        ImprovementManager.Instance.OnImprovementSelected += ImprovementManager_OnImprovementSelected;
    }

    private void ConstructPlayer() {
        playerController.Construct(
            movementSpeed: movementSpeed * movementVelocityMultiplier,
            shotSpeed: shotSpeed * shotSpeedMultiplier,
            movementSmoothing: movementSmoothing, 
            spawnPoints: spawnPoints, 
            shotProjectileSO: shotProjectileSO, 
            laserProjectileSO: laserProjectileSO, 
            breathProjectileSO: breathProjectileSO, 
            explosionProjectileSO: explosionProjectileSO,
            slowEffect: slowEnemyMultiplier, 
            explosionRangeMultiplier: explosionRangeMultiplier, 
            extrasShots: extrasShots, 
            damagesMultiplier: damagesMultiplier, 
            isPiercingShots: isPiercingShots, 
            isExplosionWithFireTrails: isExplosionWithFireTrails, 
            isLaserWithGlobalRange: isLaserWithGlobalRange, 
            isExplosionPositionFree: isExplosionPositionFree);
    }

    private void ImprovementManager_OnImprovementSelected(object sender, OnImprovementSelectedArgs args) {
        switch (args.improvementSO.improvementType) {
            case ImprovementType.MovementVelocityUp5PerCent:
                movementVelocityMultiplier = 1 + (args.count * 0.05f);
                break;
            case ImprovementType.MaxLifeUp10PerCent:
                maxLifesMultiplier = 1 + (args.count * 0.1f);
                maxLifes *= maxLifesMultiplier;
                lifes *= maxLifesMultiplier;
                break;
            case ImprovementType.FireRateUp10PerCent:
                shotSpeedMultiplier = 1 + (args.count * 0.1f);
                break;
            case ImprovementType.SlowDownEnemy5PerCentFor3Seconds:
                slowEnemyMultiplier = 1 + (args.count * 0.05f);
                break;
            case ImprovementType.Heal5PerCentMaxLifeOnKill:
                recoverLifeMultiplierOnKill = 1 + (args.count * 0.05f);
                break;
            case ImprovementType.IncreaseExplosionSpellRange10PerCent:
                explosionRangeMultiplier = 1 + (args.count * 0.1f);
                break;
            case ImprovementType.ExtraShot:
                extrasShots += args.count;
                break;
            case ImprovementType.ExtraDamage:
                damagesMultiplier = 1 + (args.count * 1f);
                break;
            case ImprovementType.PiercingShot:
                isPiercingShots = true;
                break;
            case ImprovementType.ExplosionSpellFireTrail:
                isExplosionWithFireTrails = true;
                break;
            case ImprovementType.LaserSpellGlobalRange:
                isLaserWithGlobalRange = true;
                OnLaserGlobalRangeChanged?.Invoke(this, new OnLaserGlobalRangeChangedArgs {
                    isActive = isLaserWithGlobalRange
                });
                break;
            case ImprovementType.ChoosePositionOnExplosionSpell:
                isExplosionPositionFree = true;
                OnExplosionPositionFreeChanged?.Invoke(this, new OnExplosionPositionFreeChangedArgs {
                    isActive = isExplosionPositionFree
                });
                break;
            case ImprovementType.Shield20PerCentMaxLife:
                isShieldActivatable = true;
                break;
        }

        ConstructPlayer();
    }

    private IEnumerator InvecibilityDelay() {
        isInvecible = true;
        InvokeRepeating("ToggleVisibility", 0f, 0.1f);
        yield return new WaitForSeconds(invecibilityDelay);
        CancelInvoke("ToggleVisibility");
        spriteRenderer.enabled = true;
        isInvecible = false;
    }

    private void ToggleVisibility() {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void NotifyLifeChange() {
        OnPlayerLifeChanged?.Invoke(this, new OnPlayerLifeChangedArgs {
            currentLifeNormalized = this.lifes / this.maxLifes
        });
    }

    public void Hurt(float amount) {
        if (isGodModeEnable) {
            return;
        }

        if (!isInvecible) {
            if (isShieldActivatable && shieldLife > 0) {
                shieldLife -= amount;
                
                if (shieldLife < 0) {;
                    amount = -shieldLife;
                } else {
                    return;
                }
            }

            lifes -= amount;
            lifes = Mathf.Max(0, lifes);

            StartCoroutine(InvecibilityDelay());
        }

        NotifyLifeChange();
    }

    public void Heal(float amount) {
        lifes += amount;
        lifes = Mathf.Min(maxLifes, lifes);

        NotifyLifeChange();
    }

    public void HealForKill() {
        lifes *= recoverLifeMultiplierOnKill;
        lifes = Mathf.Min(maxLifes, lifes);

        NotifyLifeChange();
    }

    public void ActivateShield() {
        if (!isShieldActivatable) {
            return;
        }

        shieldLife = maxLifes * 0.2f;
    }

    public bool IsDead() {
        return lifes <= 0;
    }

    public PlayerController GetPlayerController() {
        return playerController;
    }

    public void Move(Vector2 movement) {
        playerController.Move(movement);
    }

    public void ToggleGodMode_DEBUG() {
        isGodModeEnable = !isGodModeEnable;
    }

    public string GetImprovementsData_DEBUG() {
        return $@"movementVelocityMultiplier: {movementVelocityMultiplier}
maxLifesMultiplier: {maxLifesMultiplier}
shotSpeedMultiplier: {shotSpeedMultiplier}
slowEnemyMultiplier: {slowEnemyMultiplier}
recoverLifeMultiplierOnKill: {recoverLifeMultiplierOnKill}
explosionRangeMultiplier: {explosionRangeMultiplier}
extrasShots: {extrasShots}
damagesMultiplier: {damagesMultiplier}
isPiercingShots: {isPiercingShots}
isExplosionWithFireTrails: {isExplosionWithFireTrails}
isLaserWithGlobalRange: {isLaserWithGlobalRange}
isExplosionPositionFree: {isExplosionPositionFree}
isShieldActivatable: {isShieldActivatable}";
    }

    public string GetPlayerData_DEBUG() {
        return $@"lifes: {lifes}
maxLifes: {maxLifes}
shieldLife: {shieldLife}
isGodModeEnable: {isGodModeEnable}
isInvecible: {isInvecible}";
    }
}
