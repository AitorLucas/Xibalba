using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShot))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
        
    [Range(3, 30f)] [SerializeField] private float movementSpeed = 4f;
    [Range(3, 30f)] [SerializeField] private float shotSpeed = 4f;
    
    private PlayerMovement playerMovement;
    private PlayerShot playerShot;
    private Rigidbody2D playerRigidbody;

    public bool isMoving { get; private set; } = false;
    public bool isShooting { get; private set; } = false;
    private Vector2 movementVector = Vector2.zero;
    private Vector2 shotVector = Vector2.zero;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        playerShot = GetComponent<PlayerShot>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        movementVector = InputManager.Instance.GetMovementVectorNormalized();
        isMoving = (movementVector != Vector2.zero);

        shotVector = InputManager.Instance.GetShotVectorNormalized() * shotSpeed;
        isShooting = (shotVector != Vector2.zero); 
    }

    private void FixedUpdate() {
        if (isMoving) {
            playerMovement.Move(movementVector * Time.fixedDeltaTime, movementSpeed);
        } else {
            playerMovement.Stop();
        }

        if (isShooting) {
            playerShot.Shoot(shotVector * Time.fixedDeltaTime, playerRigidbody.velocity, shotSpeed);
        }
    }
}
