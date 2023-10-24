using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShot))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    
    private PlayerMovement playerMovement;
    private PlayerShot playerShot;
    private Rigidbody2D playerRigidbody;

    public event EventHandler<OnSpellTypeChangedArgs> OnSpellTypeChanged;
    public class OnSpellTypeChangedArgs : EventArgs {
        public SpellType spellType;
    }

    public bool isMoving { get; private set; } = false;
    public bool isShootingSpell { get; private set; } = false;
    private Vector2 movementVector = Vector2.zero;
    private Vector2 shotVector = Vector2.zero;
    private SpellType currentSpellState = SpellType.None;

    public void Construct(float movementSpeed, float shotSpeed, float movementSmoothing, Transform[] spawnPoints, ProjectileSO shotProjectileSO, ProjectileSO laserProjectileSO, ProjectileSO breathProjectileSO, ProjectileSO explosionProjectileSO, float slowEffect, float explosionRangeMultiplier, float extrasShots, float damagesMultiplier, bool isPiercingShots, bool isExplosionWithFireTrails, bool isLaserWithGlobalRange, bool isExplosionPositionFree) {
        Cascate(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed, spawnPoints: spawnPoints, shotProjectileSO: shotProjectileSO, laserProjectileSO: laserProjectileSO, breathProjectileSO: breathProjectileSO, explosionProjectileSO: explosionProjectileSO, shotSpeed: shotSpeed, slowEffect: slowEffect, explosionRangeMultiplier: explosionRangeMultiplier, extrasShots: extrasShots, damagesMultiplier: damagesMultiplier, isPiercingShots: isPiercingShots, isExplosionWithFireTrails: isExplosionWithFireTrails, isLaserWithGlobalRange: isLaserWithGlobalRange, isExplosionPositionFree: isExplosionPositionFree);
    }

    private void Cascate(float movementSmoothing, float movementSpeed, Transform[] spawnPoints, ProjectileSO shotProjectileSO, ProjectileSO laserProjectileSO, ProjectileSO breathProjectileSO, ProjectileSO explosionProjectileSO, float shotSpeed, float slowEffect, float explosionRangeMultiplier, float extrasShots, float damagesMultiplier, bool isPiercingShots, bool isExplosionWithFireTrails, bool isLaserWithGlobalRange, bool isExplosionPositionFree) {
        playerMovement.Construct(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed);
        playerShot.Construct(spawnPoints: spawnPoints, shotProjectileSO: shotProjectileSO, laserProjectileSO: laserProjectileSO, breathProjectileSO: breathProjectileSO, explosionProjectileSO: explosionProjectileSO, shotSpeed: shotSpeed, slowEffect: slowEffect, explosionRangeMultiplier: explosionRangeMultiplier, extrasShots: extrasShots, damagesMultiplier: damagesMultiplier, isPiercingShots: isPiercingShots, isExplosionWithFireTrails: isExplosionWithFireTrails, isLaserWithGlobalRange: isLaserWithGlobalRange, isExplosionPositionFree: isExplosionPositionFree);
    }

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();   
        playerShot = GetComponent<PlayerShot>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        InputManager.Instance.OnPlayerSpellCastAction += InputManager_OnPlayerSpellCastAction;
        InputManager.Instance.OnPlayerBreathSpellSelectedAction += InputManager_OnPlayerBreathSpellSelectedAction;
        InputManager.Instance.OnPlayerLaserSpellSelectedAction += InputManager_OnPlayerLaserSpellSelectedAction;
        InputManager.Instance.OnPlayerExplosionSpellSelectedAction += InputManager_OnPlayerExplosionSpellSelectedAction;
        InputManager.Instance.OnPlayerDashAction += InputManager_OnPlayerDashAction;
    }

    private void Update() {
        movementVector = InputManager.Instance.GetMovementVectorNormalized();

        Vector2 mousePosition = InputManager.Instance.GetShotDirectionVector();
        Vector2 playerPosition = transform.position;
        Vector2 mouseInWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        mouseInWorldPosition = mouseInWorldPosition - playerPosition;
        shotVector = mouseInWorldPosition.normalized;
    }

    private void FixedUpdate() {
        isMoving = (playerRigidbody.velocity != Vector2.zero) || (movementVector != Vector2.zero);

        if (isMoving && !isShootingSpell) {
            playerMovement.Move(movementVector * Time.fixedDeltaTime);
        } else {
            playerMovement.Stop();
        }
    }

    private void InputManager_OnPlayerSpellCastAction(object sender, EventArgs e) {
        if (GameManager.Instance.IsPaused()) {
            return;
        }

        if (currentSpellState == SpellType.None) {
            playerShot.ShotBurst(shotVector * Time.fixedDeltaTime, playerRigidbody.velocity);
        } else {
            playerShot.Spell(spellDirection: shotVector, spellType: currentSpellState);
            currentSpellState = SpellType.None;
            NotifySpellTypeChange();
            StartCoroutine(SpellDelay());
        }
    }
       
    private void InputManager_OnPlayerBreathSpellSelectedAction(object sender, EventArgs e) {
        if (GameManager.Instance.IsPaused()) {
            return;
        }

        if (currentSpellState == SpellType.Breath) {
            currentSpellState = SpellType.None;
        } else {
            currentSpellState = SpellType.Breath;
        }
        NotifySpellTypeChange();
    }

    private void InputManager_OnPlayerLaserSpellSelectedAction(object sender, EventArgs e) {
        if (GameManager.Instance.IsPaused()) {
            return;
        }

        if (currentSpellState == SpellType.Laser) {
            currentSpellState = SpellType.None;
        } else {
            currentSpellState = SpellType.Laser;
        }
        NotifySpellTypeChange();
    }

    private void InputManager_OnPlayerExplosionSpellSelectedAction(object sender, EventArgs e) {
        if (GameManager.Instance.IsPaused()) {
            return;
        }

        if (currentSpellState == SpellType.Explosion) {
            currentSpellState = SpellType.None;
        } else {
            currentSpellState = SpellType.Explosion;
        }
        NotifySpellTypeChange();
    }

    private void InputManager_OnPlayerDashAction(object sender, EventArgs e) {
        if (GameManager.Instance.IsPaused()) {
            return;
        }
        
        playerMovement.Dash(movementVector);
    }

    private IEnumerator SpellDelay() {
        isShootingSpell = true;
        yield return new WaitForSeconds(0.4f);
        isShootingSpell = false;
    }

    private void NotifySpellTypeChange() {
        OnSpellTypeChanged?.Invoke(this, new OnSpellTypeChangedArgs { spellType = this.currentSpellState });
    }

    public void Move(Vector2 movement) {
        playerMovement.Move(movement);
    }
}
