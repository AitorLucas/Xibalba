using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyCollision))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour {

    [SerializeField] private float maxLifes = 3f;
    [SerializeField] private int experience = 10;
    [Header("Controller")]
    [Range(0, 10f)][SerializeField] private float followRange = 8f;
    [Range(0, 5f)][SerializeField] private float shotRange = 4f;
    [Header("Movement")]
    [Range(1, 30f)][SerializeField] private float movementSpeed = 1f;
    [Range(0, .5f)][SerializeField] private float movementSmoothing = .3f;
    [Header("Colission")]
    [SerializeField] private float damageCollision  = 0.5f;
    [Header("Shoot")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private ProjectileSO shotProjectileSO;
    [Range(3, 30f)][SerializeField] private float shotSpeed = 5f;

    // - Events
    public event EventHandler<OnEnemyDestroyArgs> OnEnemyDestroy;
    public class OnEnemyDestroyArgs : EventArgs {
        public float experience;
    }
    // - Required
    private EnemyController enemyController;
    private EnemyCollision enemyCollision;
    private EnemyMovement enemyMovement;

    private float lifes;

    private void Awake() {
        lifes = maxLifes;

        enemyController = GetComponent<EnemyController>();
        enemyCollision = GetComponent<EnemyCollision>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Start() {
        enemyController.Construct(followRange: followRange, shotRange: shotRange, movementSmoothing: movementSmoothing, movementSpeed: movementSpeed, spawnPoints: spawnPoints, shotProjectileSO: shotProjectileSO, shotSpeed: shotSpeed);
        enemyCollision.Construct(damageCollision: damageCollision);
    }

    public void Hurt(float amount) {
        lifes -= amount;

        if (lifes <= 0) {
            enemyMovement.DeathAnimate();
            enemyController.Kill();

            enemyCollision.DisableCollision();

            OnEnemyDestroy?.Invoke(this, new OnEnemyDestroyArgs { 
                experience = this.experience
            });

            Destroy(gameObject, 0.6f); // Death animation = 0.6s
        } else {
            StartCoroutine(StartHurtingEffect());
        }
    }

    public void AddKnockback(Vector2 direction) {
        enemyMovement.AddKnockback(direction);
    }

    public void AddSlowEffect(float slowEffect) {
        enemyMovement.AddSlowEffect(slowEffect);
    }

    private IEnumerator StartHurtingEffect() {
        Material normalMaterial = GetComponent<Renderer>().material;
        Material redMaterial = normalMaterial;
        redMaterial.SetFloat("_Alpha", 0.4f);
        normalMaterial.Lerp(normalMaterial, redMaterial, 0.3f);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(StopHurtingEffect());
    }

    private IEnumerator StopHurtingEffect() {
        Material redMaterial = GetComponent<Renderer>().material;
        Material normalMaterial = redMaterial;
        normalMaterial.SetFloat("_Alpha", 0f);
        redMaterial.Lerp(redMaterial, normalMaterial, 0.3f);
        yield return new WaitForSeconds(0.3f);
    }
}
