using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : ISingleton<Player> {

    [Range(0, 10f)][SerializeField] private float maxLifes = 3;
    [Range(0, 2f)][SerializeField] private float invecibilityDelay = 0.8f;
    [Header("Controller")]
    // [SerializeField] private Camera camera;
    [Header("Movement")]
    [Range(3, 30f)][SerializeField] private float movementSpeed = 6f;
    [Range(0, .5f)][SerializeField] private float movementSmoothing = .3f;
    [Header("Shoot")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Projectile projectilePreFab;
    [SerializeField] private Projectile laserPreFab;
    [SerializeField] private Projectile breathPreFab;
    [SerializeField] private Projectile explosionPreFab;
    [Range(5, 60f)][SerializeField] private float shotSpeed = 25f;
    [Range(0.1f, 3.0f)][SerializeField] private float shotDelay = 0.6f;

    public event EventHandler<OnPlayerLifeChangedArgs> OnPlayerLifeChanged;
    public class OnPlayerLifeChangedArgs : EventArgs {
        public float currentLifeNormalized;
    }

    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    private bool isGodModeEnable = true;
    private bool isInvecible = false;
    private float lifes = 3;

    protected override void Awake() {
        base.Awake();

        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        lifes = maxLifes;
    }

    private void Start() {
        playerController.Construct(movementSpeed: movementSpeed, shotSpeed: shotSpeed, movementSmoothing: movementSmoothing, spawnPoints: spawnPoints, projectilePreFab: projectilePreFab, laserPreFab: laserPreFab, breathPreFab: breathPreFab, explosionPreFab: explosionPreFab, shotDelay: shotDelay);
    }

    public void Hurt(float amount) {
        if (isGodModeEnable) {
            return;
        }

        if (!isInvecible) {
            lifes -= amount;
            lifes = Mathf.Max(0, lifes);

            isInvecible = true;
            StartCoroutine(InvecibilityDelay());
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
        InvokeRepeating("ToggleVisibility", 0f, 0.1f);
        yield return new WaitForSeconds(invecibilityDelay);
        CancelInvoke("ToggleVisibility");
        spriteRenderer.enabled = true;
        isInvecible = false;
    }

    private void ToggleVisibility() {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    public bool IsDead() {
        return lifes <= 0;
    }

    public void ToggleGodMode() {
        isGodModeEnable = !isGodModeEnable;
    }
}
