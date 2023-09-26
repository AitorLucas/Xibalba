using System;
using UnityEngine;

public enum SpellType {
    Heavy,
    Light,
    None,
};

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShot))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    
    private PlayerMovement playerMovement;
    private PlayerShot playerShot;
    private Rigidbody2D playerRigidbody;

    public bool isMoving { get; private set; } = false;
    public bool isShooting { get; private set; } = false;
    private Vector2 movementVector = Vector2.zero;
    private Vector2 shotVector = Vector2.zero;
    private SpellType currentSpellState = SpellType.None;

    public void Construct(float movementSpeed, float shotDelay, float shotSpeed, float movementSmoothing, Transform[] spawnPoints, Projectile projectilePreFab, Projectile laserPreFab, Projectile breathPreFab, Projectile explosionPreFab) {
        this.Cascate(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed, spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, laserPreFab: laserPreFab, breathPreFab: breathPreFab, explosionPreFab: explosionPreFab, shotDelay: shotDelay, shotSpeed: shotSpeed);
    }

    private void Cascate(float movementSmoothing, float movementSpeed, Transform[] spawnPoints, Projectile projectilePreFab, Projectile laserPreFab, Projectile breathPreFab, Projectile explosionPreFab, float shotDelay, float shotSpeed) {
        playerMovement.Construct(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed);
        playerShot.Construct(spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, laserPreFab: laserPreFab, breathPreFab: breathPreFab, explosionPreFab: explosionPreFab, shotDelay: shotDelay, shotSpeed: shotSpeed);
    }

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();   
        playerShot = GetComponent<PlayerShot>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        InputManager.Instance.OnPlayerSpellCastAction += InputManager_OnPlayerSpellCastAction;
        InputManager.Instance.OnPlayerLightSpellSelectedAction += InputManager_OnPlayerLightSpellSelectedAction;
        InputManager.Instance.OnPlayerHeavySpellSelectedAction += InputManager_OnPlayerHeavySpellSelectedAction;
        InputManager.Instance.OnPlayerDashAction += InputManager_OnPlayerDashAction;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Player velocity: " + playerRigidbody.velocity);
        GUILayout.EndArea();
    }

    private void Update() {
        movementVector = InputManager.Instance.GetMovementVectorNormalized();
        isMoving = (movementVector != Vector2.zero);

        Vector2 mousePosition = InputManager.Instance.GetShotDirectionVector();
        Vector2 playerPosition = transform.position;
        Vector2 mouseInWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        mouseInWorldPosition = mouseInWorldPosition - playerPosition;
        shotVector = mouseInWorldPosition.normalized;
        isShooting = (shotVector != Vector2.zero);
    }

    private void FixedUpdate() {
        if (isMoving) {
            playerMovement.Move(movementVector * Time.fixedDeltaTime);
        } else {
            playerMovement.Stop();
        }

        if (isShooting) {
            playerShot.Shot(shotVector * Time.fixedDeltaTime, playerRigidbody.velocity);
        }
    }

    private void InputManager_OnPlayerSpellCastAction(object sender, EventArgs e) {
        switch (currentSpellState) {
            case SpellType.Heavy:
                playerShot.ShotLaser(shotVector, playerRigidbody.velocity);
                break;
            case SpellType.Light:
                playerShot.ShotLaser(shotVector, playerRigidbody.velocity);
                break;
            case SpellType.None:
                // playerShot.ShotExplosion();
                break;
        }
    }

    private void InputManager_OnPlayerLightSpellSelectedAction(object sender, EventArgs e) {
        currentSpellState = SpellType.Light;
    }

    private void InputManager_OnPlayerHeavySpellSelectedAction(object sender, EventArgs e) {
        currentSpellState = SpellType.Heavy;
    }

    private void InputManager_OnPlayerDashAction(object sender, EventArgs e) {
        playerMovement.Dash(movementVector);
    }
}
