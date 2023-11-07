using System;
using System.Collections;
using UnityEngine;

public enum ProjectileFrom {
    Enemy,
    Player,
};

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour {

    // - Constructed
    private float lifeTime;
    private float damage;
    private ProjectileFrom wasShootBy;
    private float slowEffect;
    private int piercingShots;
    private bool isSpell;

    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;

    public event EventHandler OnProjectileHit;
    private int piercingCount = 0;

    public void Construct(float lifeTime, float damage, ProjectileFrom wasShootBy, float slowEffect, int piercingShots, bool isSpell) {
        this.lifeTime = lifeTime;
        this.damage = damage;
        this.wasShootBy = wasShootBy;
        this.slowEffect = slowEffect;
        this.piercingShots = piercingShots;
        this.isSpell = isSpell;
    }

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();

        SoundManager.Instance.RegisterNewProjectile(this);
    }

    private void OnDestroy() {
        SoundManager.Instance.UnregisterProjectile(this);
    }

    private void Start() {
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay() {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider) {  
        switch (wasShootBy) {
            case (ProjectileFrom.Enemy):
                if (collider.transform.TryGetComponent(out Player player)) {
                    player.Hurt(damage);
                    // if(!isPiercingShots) {
                        // if (!isSpell) {
                            Destroy(gameObject);
                        // }
                    // }
                }
                break;
            case (ProjectileFrom.Player):
                if (collider.transform.TryGetComponent(out Enemy enemy)) {
                    enemy.Hurt(damage);
                    enemy.AddKnockback(rigidbody2D.velocity.normalized);
                    enemy.AddSlowEffect(slowEffect);

                    if (piercingCount < piercingShots) {
                        piercingCount++;
                    } else {
                        if (!isSpell) {
                            Destroy(gameObject);
                            OnProjectileHit?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
                break;
        }

        if (collider.transform.TryGetComponent(out Obstacle obstacle)) {
            obstacle.AddKnockback(rigidbody2D.velocity.normalized);
            if (!isSpell) {
                Destroy(gameObject);
                OnProjectileHit?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void DisableCollision() {
        collider2D.enabled = false;
    }

    public Rigidbody2D GetRigidbody2D() {
        return rigidbody2D;
    }
}
