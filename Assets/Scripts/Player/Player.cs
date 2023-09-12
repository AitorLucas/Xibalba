using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : ISingleton<Player> {

    [Range(0, 10f)][SerializeField] private float maxLifes = 3;
    [Range(0, 2f)][SerializeField] private float invecibilityDelay = 0.5f;
    [Header("Controller")]
    [SerializeField] private Camera camera;
    [Header("Movement")]
    [Range(3, 30f)][SerializeField] private float movementSpeed = 4f;
    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;
    [Header("Shoot")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Projectile projectilePreFab;
    [Range(3, 30f)][SerializeField] private float shotSpeed = 5f;
    [Range(0.1f, 3.0f)][SerializeField] private float shotDelay = 0.7f;

    public event EventHandler<OnPlayerLifeChangedArgs> OnPlayerLifeChanged;
    public class OnPlayerLifeChangedArgs : EventArgs {
        public float currentLifeNormalized;
    }

    private PlayerController playerController;

    private bool isInvecible = false;
    private float lifes = 3;

    protected override void Awake() {
        base.Awake();

        playerController = GetComponent<PlayerController>();

        lifes = maxLifes;
    }

    private void Start() {
        playerController.Construct(camera: camera, movementSpeed: movementSpeed, shotSpeed: shotSpeed, movementSmoothing: movementSmoothing, spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, shotDelay: shotDelay);
    }

    public void Hurt(float amount) {
        if (!isInvecible) {
            lifes -= amount;
            lifes = Mathf.Max(0, lifes);

            StartCoroutine(InvecibilityDelay());
            isInvecible = true;
        }

        OnPlayerLifeChanged?.Invoke(this, new OnPlayerLifeChangedArgs {
            currentLifeNormalized = this.lifes / this.maxLifes
        });
    }

    public void Heal(float amount) {
        lifes += amount;
        lifes = Mathf.Min(maxLifes, lifes);

        OnPlayerLifeChanged?.Invoke(this, new OnPlayerLifeChangedArgs {
            currentLifeNormalized = this.lifes / this.maxLifes
        });
    }

    private IEnumerator InvecibilityDelay() {
        yield return new WaitForSeconds(invecibilityDelay);
        isInvecible = false;
    }

    public bool IsDead() {
        return lifes <= 0;
    }
}
