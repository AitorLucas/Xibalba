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

    public event EventHandler<OnPlayerSpellTypeChangedArgs> OnPlayerSpellTypeChanged;
    public class OnPlayerSpellTypeChangedArgs : EventArgs {
        public SpellType spellType;
    }

    public bool isMoving { get; private set; } = false;
    public bool isShootingSpell { get; private set; } = false;
    private Vector2 movementVector = Vector2.zero;
    private Vector2 shotVector = Vector2.zero;
    private SpellType currentSpellState = SpellType.None;

    public void Construct(float movementSpeed, float shotDelay, float shotSpeed, float movementSmoothing, Transform[] spawnPoints, Projectile projectilePreFab, Projectile laserPreFab, Projectile breathPreFab, Projectile explosionPreFab, float slowEffect, float explosionRangeMultiplier, float extrasShots, float damagesMultiplier, bool isPiercingShots, bool isExplosionWithFireTrails, bool isLaserWithGlobalRange, bool isExplosionPositionFree) {
        Cascate(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed, spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, laserPreFab: laserPreFab, breathPreFab: breathPreFab, explosionPreFab: explosionPreFab, shotDelay: shotDelay, shotSpeed: shotSpeed, slowEffect: slowEffect, explosionRangeMultiplier: explosionRangeMultiplier, extrasShots: extrasShots, damagesMultiplier: damagesMultiplier, isPiercingShots: isPiercingShots, isExplosionWithFireTrails: isExplosionWithFireTrails, isLaserWithGlobalRange: isLaserWithGlobalRange, isExplosionPositionFree: isExplosionPositionFree);
    }

    private void Cascate(float movementSmoothing, float movementSpeed, Transform[] spawnPoints, Projectile projectilePreFab, Projectile laserPreFab, Projectile breathPreFab, Projectile explosionPreFab, float shotDelay, float shotSpeed, float slowEffect, float explosionRangeMultiplier, float extrasShots, float damagesMultiplier, bool isPiercingShots, bool isExplosionWithFireTrails, bool isLaserWithGlobalRange, bool isExplosionPositionFree) {
        playerMovement.Construct(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed);
        playerShot.Construct(spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, laserPreFab: laserPreFab, breathPreFab: breathPreFab, explosionPreFab: explosionPreFab, shotDelay: shotDelay, shotSpeed: shotSpeed, slowEffect: slowEffect, explosionRangeMultiplier: explosionRangeMultiplier, extrasShots: extrasShots, damagesMultiplier: damagesMultiplier, isPiercingShots: isPiercingShots, isExplosionWithFireTrails: isExplosionWithFireTrails, isLaserWithGlobalRange: isLaserWithGlobalRange, isExplosionPositionFree: isExplosionPositionFree);
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
        isMoving = (movementVector != Vector2.zero);

        Vector2 mousePosition = InputManager.Instance.GetShotDirectionVector();
        Vector2 playerPosition = transform.position;
        Vector2 mouseInWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        mouseInWorldPosition = mouseInWorldPosition - playerPosition;
        shotVector = mouseInWorldPosition.normalized;
    }

    private void FixedUpdate() {
        if (isMoving && !isShootingSpell) {
            playerMovement.Move(movementVector * Time.fixedDeltaTime);
        } else {
            playerMovement.Stop();
        }
    }

    private void InputManager_OnPlayerSpellCastAction(object sender, EventArgs e) {
        if (currentSpellState == SpellType.None) {
            playerShot.Shot(shotVector * Time.fixedDeltaTime, playerRigidbody.velocity);
        } else {
            playerShot.Spell(spellDirection: shotVector, spellType: currentSpellState, playerVelocity: playerRigidbody.velocity);
            currentSpellState = SpellType.None;
            NotifySpellTypeChange();
            StartCoroutine(SpellDelay());
        }
    }
       
    private void InputManager_OnPlayerBreathSpellSelectedAction(object sender, EventArgs e) {
        if (currentSpellState == SpellType.Breath) {
            currentSpellState = SpellType.None;
        } else {
            currentSpellState = SpellType.Breath;
        }
        NotifySpellTypeChange();
    }

    private void InputManager_OnPlayerLaserSpellSelectedAction(object sender, EventArgs e) {
        if (currentSpellState == SpellType.Laser) {
            currentSpellState = SpellType.None;
        } else {
            currentSpellState = SpellType.Laser;
        }
    }

    private void InputManager_OnPlayerExplosionSpellSelectedAction(object sender, EventArgs e) {
        if (currentSpellState == SpellType.Explosion) {
            currentSpellState = SpellType.None;
        } else {
            currentSpellState = SpellType.Explosion;
        }
    }

    private void InputManager_OnPlayerDashAction(object sender, EventArgs e) {
        playerMovement.Dash(movementVector);
    }

    private IEnumerator SpellDelay() {
        isShootingSpell = true;
        yield return new WaitForSeconds(0.4f);
        isShootingSpell = false;
    }

    private void NotifySpellTypeChange() {
        OnPlayerSpellTypeChanged?.Invoke(this, new OnPlayerSpellTypeChangedArgs { spellType = currentSpellState });
    }
}
