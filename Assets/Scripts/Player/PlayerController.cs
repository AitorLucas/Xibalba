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
    
    // - Constructed
    private Camera camera;

    private PlayerMovement playerMovement;
    private PlayerShot playerShot;
    private Rigidbody2D playerRigidbody;

    public bool isMoving { get; private set; } = false;
    public bool isShooting { get; private set; } = false;
    private Vector2 movementVector = Vector2.zero;
    private Vector2 shotVector = Vector2.zero;
    private SpellType currentSpellState = SpellType.None;

    public void Construct(Camera camera, float movementSpeed, float shotDelay, float shotSpeed, float movementSmoothing, Transform[] spawnPoints, Projectile projectilePreFab) {
        this.camera = camera;
        this.Cascate(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed, spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, shotDelay: shotDelay, shotSpeed: shotSpeed);
    }

    private void Cascate(float movementSmoothing, float movementSpeed, Transform[] spawnPoints, Projectile projectilePreFab, float shotDelay, float shotSpeed) {
        playerMovement.Construct(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed);
        playerShot.Construct(spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, shotDelay: shotDelay, shotSpeed: shotSpeed);
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

    private void Update() {
        movementVector = InputManager.Instance.GetMovementVectorNormalized();
        isMoving = (movementVector != Vector2.zero);

        // Debug.Log("Mouse: " + InputManager.Instance.GetShotDirectionVector() + Mouse.current.position.ReadValue());
        Debug.Log("Mouse: " + InputManager.Instance.GetShotDirectionVector());

        // Vector3 camDis = new Vector3(InputManager.Instance.GetShotDirectionVector().x, InputManager.Instance.GetShotDirectionVector().y, camera.transform.position.z);
        // Vector3 mousepos = camera.ScreenToWorldPoint(camDis);

         Vector2 mousePosition = InputManager.Instance.GetShotDirectionVector();

        Ray camRay = camera.ScreenPointToRay(mousePosition);
        Debug.DrawRay(camRay.origin, camRay.direction * 1000, Color.red);

        // Debug.Log("CAM: " + mousepos);

        // shotVector = -mousepos.normalized;

        shotVector = InputManager.Instance.GetShotVectorNormalized();
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

    }

    private void InputManager_OnPlayerLightSpellSelectedAction(object sender, EventArgs e) {
        currentSpellState = SpellType.Light;
    }

    private void InputManager_OnPlayerHeavySpellSelectedAction(object sender, EventArgs e) {
        currentSpellState = SpellType.Heavy;
    }

    private void InputManager_OnPlayerDashAction(object sender, EventArgs e) {
        
    }
}
