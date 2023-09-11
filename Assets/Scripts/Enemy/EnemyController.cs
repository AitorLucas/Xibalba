using System.Collections;
using UnityEngine;

public enum EnemyState {
    Wander,
    Follow,
    Shoot,
    Die,
};

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyShot))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour {

    // - Constructed
    private float followRange;
    private float shotRange;    

    public EnemyMovement enemyMovement;
    public EnemyShot enemyShot;
    private Rigidbody2D enemyRigidbody;

    private Player player;

    private EnemyState currentState = EnemyState.Wander;
    private Vector2 newDirection;
    private bool chooseDirection = false;

    public void Construct(float followRange, float shotRange, float movementSmoothing, float movementSpeed, Transform[] spawnPoints, Projectile projectilePreFab, float shotDelay, float shotSpeed) {
        this.followRange = followRange;
        this.shotRange = shotRange;
        this.Cascate(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed, spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, shotDelay: shotDelay, shotSpeed: shotSpeed);
    }

    public void Cascate(float movementSmoothing, float movementSpeed, Transform[] spawnPoints, Projectile projectilePreFab, float shotDelay, float shotSpeed) {
        enemyMovement.Construct(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed);
        enemyShot.Construct(spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, shotDelay: shotDelay, shotSpeed: shotSpeed);
    }

    private void Awake() {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyShot = GetComponent<EnemyShot>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        player = Player.Instance;
    }

    private void Update() {
        if (currentState != EnemyState.Die) {
            if (IsPlayerInRange(followRange) && !IsPlayerInRange(shotRange)) {
                currentState = EnemyState.Follow;
            } else if (IsPlayerInRange(shotRange)) {
                currentState = EnemyState.Shoot;
            } else {
                currentState = EnemyState.Wander;
            }
        }
    }

    private void FixedUpdate() {
        switch(currentState) {
            case (EnemyState.Wander):
                if (!chooseDirection) {
                    StartCoroutine(ChooseDirection());
                }
                
                enemyMovement.Move(newDirection * Time.fixedDeltaTime);
                break;
            case (EnemyState.Follow):
                newDirection = (player.transform.position - transform.position).normalized;
                enemyMovement.Move(newDirection * Time.fixedDeltaTime);
                break;
            case (EnemyState.Shoot):
                enemyMovement.Stop();
                Vector2 shotDirection = (player.transform.position - transform.position).normalized;
                enemyShot.Shot(shotDirection * Time.fixedDeltaTime, enemyRigidbody.velocity);
                break;
            case (EnemyState.Die):
                enemyMovement.Stop();
                break;
        }
    }

    private bool IsPlayerInRange(float range) {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection() {
        chooseDirection = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
  
        bool isXAxis = Random.Range(0f, 1f) >= 0.5f;
        int[] possibleValues = {-1, 1};
        int newValue = possibleValues[Random.Range(0, possibleValues.Length)];

        newDirection = new Vector2(
            isXAxis ? newValue : 0,
            isXAxis ? 0 : newValue);
        chooseDirection = false;
    }

    public void InvertMovingDirection() {
        newDirection = -newDirection;
    }

    public void Kill() {
        currentState = EnemyState.Die;
    }
}
