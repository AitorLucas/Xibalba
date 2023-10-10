using System.Collections;
using UnityEngine;
using Pathfinding;
using UnityEngine.Video;

public enum EnemyState {
    Wander,
    Follow,
    Shoot,
    Die,
};

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyShot))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyController : MonoBehaviour {

    // - Constructed
    private float followRange;
    private float shotRange;    

    private Seeker seeker;
    private EnemyMovement enemyMovement;
    private EnemyShot enemyShot;
    private Rigidbody2D enemyRigidbody;

    private Player player;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachEndOfPath = false;
    private float nextWaypointDistance = 1f;
    private EnemyState currentState = EnemyState.Wander;
    private Vector2 newDirection;
    private bool chooseDirection = false;

    public void Construct(float followRange, float shotRange, float movementSmoothing, float movementSpeed, Transform[] spawnPoints, ProjectileSO shotProjectileSO, float shotSpeed) {
        this.followRange = followRange;
        this.shotRange = shotRange;
        this.Cascate(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed, spawnPoints: spawnPoints, shotProjectileSO: shotProjectileSO, shotSpeed: shotSpeed);
    }

    public void Cascate(float movementSmoothing, float movementSpeed, Transform[] spawnPoints, ProjectileSO shotProjectileSO, float shotSpeed) {
        enemyMovement.Construct(movementSmoothing: movementSmoothing, movementSpeed: movementSpeed);
        enemyShot.Construct(spawnPoints: spawnPoints, shotProjectileSO: shotProjectileSO, shotSpeed: shotSpeed);
    }

    private void Awake() {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyShot = GetComponent<EnemyShot>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    private void Start() {
        player = Player.Instance;

        InvokeRepeating("UpdatePath", 0f, 0.5f); 
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

        if (path != null) {
            if (currentWaypoint >= path.vectorPath.Count) {
                reachEndOfPath = true;
                return;
            } else {
                reachEndOfPath = false;
            }

            float distance = Vector2.Distance(enemyRigidbody.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) {
                currentWaypoint += 1;
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
                if (path == null && !reachEndOfPath) {
                    break;
                }

                newDirection = ((Vector2)path.vectorPath[currentWaypoint] - enemyRigidbody.position).normalized; // ITS CAUSING AN ERROR AND FRAME DROPS
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

    private void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(enemyRigidbody.position, player.transform.position, OnPathComplete);
        }
    }  

    private void OnPathComplete(Path path) {
        if (!path.error) {
            this.path = path;
            currentWaypoint = 0; 
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