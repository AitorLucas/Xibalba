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
    private bool isPiercingShots;
    private bool isSpell;

    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;

    public void Construct(float lifeTime, float damage, ProjectileFrom wasShootBy, float slowEffect, bool isPiercingShots, bool isSpell) {
        this.lifeTime = lifeTime;
        this.damage = damage;
        this.wasShootBy = wasShootBy;
        this.slowEffect = slowEffect;
        this.isPiercingShots = isPiercingShots;
        this.isSpell = isSpell;
    }

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
                    if(!isPiercingShots) {
                        if (!isSpell) {
                            Destroy(gameObject);
                        }
                    }
                }
                break;
            case (ProjectileFrom.Player):
                if (collider.transform.TryGetComponent(out Enemy enemy)) {
                    enemy.Hurt(damage);
                    enemy.AddKnockback(rigidbody2D.velocity.normalized);
                    enemy.AddSlowEffect(slowEffect);
                    if(!isPiercingShots) {
                        if (!isSpell) {
                            Destroy(gameObject);
                        }  
                    }
                }
                break;
        }

        if (collider.transform.TryGetComponent(out Obstacle obstacle)) {
            obstacle.AddKnockback(rigidbody2D.velocity.normalized);
            if (!isSpell) {
                Destroy(gameObject);
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