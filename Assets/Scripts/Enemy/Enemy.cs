using System;
using UnityEngine;

[RequireComponent(typeof(EnemyCollision))]
[RequireComponent(typeof(EnemyController))]
public class Enemy : MonoBehaviour {

    [SerializeField] private float maxLifes = 3f;
    [SerializeField] private int experience = 10;
    [Header("Controller")]
    [Range(2, 10f)][SerializeField] private float followRange = 8f;
    [Range(1, 5f)][SerializeField] private float shotRange = 4f;
    [Header("Movement")]
    [Range(1, 30f)][SerializeField] private float movementSpeed = 1f;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;
    [Header("Colission")]
    [SerializeField] private float damageCollision  = 0.5f;
    [Header("Shoot")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Projectile projectilePreFab;
    [Range(3, 30f)][SerializeField] private float shotSpeed = 5f;
    [Range(0.1f, 3.0f)][SerializeField] private float shotDelay = 0.7f;
    // [SerializeField] private float shotDamage = 1f;
    // - Events
    public event EventHandler<OnEnemyDestroyArgs> OnEnemyDestroy;
    public class OnEnemyDestroyArgs : EventArgs {
        public float experience;
    }
    // - Required
    private EnemyController enemyController;
    private EnemyCollision enemyCollision;

    private float lifes;

    private void Awake() {
        lifes = maxLifes;

        enemyController = GetComponent<EnemyController>();
        enemyCollision = GetComponent<EnemyCollision>();
    }

    private void Start() {
        enemyController.Construct(followRange: followRange, shotRange: shotRange, movementSmoothing: movementSmoothing, movementSpeed: movementSpeed, spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, shotDelay: shotDelay, shotSpeed: shotSpeed);
        enemyCollision.Construct(damageCollision: damageCollision);
    }

    public void Hurt(float amount) {
        lifes -= amount;

        if (lifes <= 0) {
            enemyController.enemyMovement.DeathAnimate();
            enemyController.Kill();

            enemyCollision.DisableCollision();

            OnEnemyDestroy?.Invoke(this, new OnEnemyDestroyArgs { 
                experience = this.experience
            });

            Destroy(gameObject, 0.6f); // Death animation = 0.6s
        }
    }
}
