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
        InputManager.Instance.OnSpellCastAction += InputManager_OnSpellCastAction;
        InputManager.Instance.OnLightSpellSelectedAction += InputManager_OnLightSpellSelectedAction;
        InputManager.Instance.OnHeavySpellSelectedAction += InputManager_OnHeavySpellSelectedAction;
        InputManager.Instance.OnDashAction += InputManager_OnDashAction;
    }

    private void Update() {
        movementVector = InputManager.Instance.GetMovementVectorNormalized();
        isMoving = (movementVector != Vector2.zero);

        // Debug.Log("Mouse: " + InputManager.Instance.GetShotDirectionVector() + Mouse.current.position.ReadValue());
        // Debug.Log("Mouse: " + InputManager.Instance.GetShotDirectionVector());

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

    private void InputManager_OnSpellCastAction(object sender, EventArgs e) {

    }

    private void InputManager_OnLightSpellSelectedAction(object sender, EventArgs e) {
        currentSpellState = SpellType.Light;
    }

    private void InputManager_OnHeavySpellSelectedAction(object sender, EventArgs e) {
        currentSpellState = SpellType.Heavy;
    }

    private void InputManager_OnDashAction(object sender, EventArgs e) {

    }
}
