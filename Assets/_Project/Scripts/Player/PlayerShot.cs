using System;
using System.Collections;
using UnityEngine;

public class PlayerShot : MonoBehaviour {

    // - Constructed
    private Transform[] spawnPoints;
    private Projectile projectilePreFab;
    private Projectile laserPreFab;
    private Projectile breathPreFab;
    private Projectile explosionPreFab;
    private float shotDelay;
    private float shotSpeed;
    private float slowEffect;
    private float explosionRangeMultiplier;
    private float extrasShots;
    private float damagesMultiplier;
    private bool isPiercingShots;
    private bool isExplosionWithFireTrails;
    private bool isLaserWithGlobalRange;
    private bool isExplosionPositionFree;

    private Coroutine shotCoroutine = null;
    private bool canShoot = true;
    private bool canShootLaser = true;
    private bool canShootBreath = true;
    private bool canShootExplosion = true;

    public void Construct(Transform[] spawnPoints, Projectile projectilePreFab, Projectile laserPreFab, Projectile breathPreFab, Projectile explosionPreFab, float shotDelay, float shotSpeed, float slowEffect, float explosionRangeMultiplier, float extrasShots, float damagesMultiplier, bool isPiercingShots, bool isExplosionWithFireTrails, bool isLaserWithGlobalRange, bool isExplosionPositionFree) {
        this.spawnPoints = spawnPoints;
        this.projectilePreFab = projectilePreFab;
        this.laserPreFab = laserPreFab;
        this.breathPreFab = breathPreFab;
        this.explosionPreFab = explosionPreFab;
        this.shotDelay = shotDelay;
        this.shotSpeed = shotSpeed;
        this.slowEffect = slowEffect;
        this.explosionRangeMultiplier = explosionRangeMultiplier;
        this.extrasShots = extrasShots;
        this.damagesMultiplier = damagesMultiplier;
        this.isPiercingShots = isPiercingShots;
        this.isExplosionWithFireTrails = isExplosionWithFireTrails;
        this.isLaserWithGlobalRange = isLaserWithGlobalRange;
        this.isExplosionPositionFree = isExplosionPositionFree;
    }

    public void Shot(Vector2 shotDirection, Vector2 playerVelocity) {
        if (canShoot) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(projectilePreFab, spawnpoint.position, Quaternion.Euler(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg));
                projectile.Construct(lifeTime: 1f, damage: 1f * damagesMultiplier, wasShootBy: ProjectileFrom.Player, slowEffect: slowEffect, isPiercingShots: isPiercingShots);

                Rigidbody2D projectileRigidBody = projectile.GetRigidbody2D();

                projectileRigidBody.velocity = playerVelocity / 3; // Adds a bit of starting speed according to player movement
                
                float kMagicNumber = 10f;
                projectileRigidBody.AddForce(shotDirection * shotSpeed * kMagicNumber, ForceMode2D.Impulse);
            }

            if (shotCoroutine != null) {
                StopCoroutine(shotCoroutine);
            }
            canShoot = false;
            shotCoroutine = StartCoroutine(ShotDelay());
        }
    }

    private void ShotLaser(Vector2 shotDirection, Vector2 playerVelocity) {
        if (canShootLaser) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(laserPreFab, (Vector2)spawnpoint.position + (shotDirection * 0.6f), Quaternion.Euler(new Vector3(0, 0, -90)));
                projectile.Construct(lifeTime: 0.25f, damage: 3f * damagesMultiplier, wasShootBy: ProjectileFrom.Player, slowEffect: slowEffect, isPiercingShots: true);
                projectile.transform.Rotate(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg);

                Rigidbody2D projectileRigidBody = projectile.GetRigidbody2D();
                projectileRigidBody.velocity = playerVelocity; // Same velocity as player
            }
            
            StopCoroutine(shotCoroutine);
            canShoot = false;
            canShootLaser = false;
            shotCoroutine = StartCoroutine(ShotDelay());
            StartCoroutine(ShotLaserDelay());
        }
    }

    private void ShotBreath(Vector2 shotDirection, Vector2 playerVelocity) {
        if (canShootBreath) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(breathPreFab, (Vector2)spawnpoint.position + (shotDirection * 0.6f), Quaternion.Euler(new Vector3(0, 0, -90)));
                projectile.Construct(lifeTime: 0.4f, damage: 3f * damagesMultiplier, wasShootBy: ProjectileFrom.Player, slowEffect: slowEffect, isPiercingShots: true);
                projectile.transform.Rotate(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg);

                Rigidbody2D projectileRigidBody = projectile.GetRigidbody2D();
                projectileRigidBody.velocity = playerVelocity; // Same velocity as player
            }
            
            StopCoroutine(shotCoroutine);
            canShoot = false;
            canShootBreath = false;
            shotCoroutine = StartCoroutine(ShotDelay());
            StartCoroutine(ShotBreathDelay());

            Player.Instance.ActivateShield();
        }
    }

    private void ShotExplosion(Vector2 shotDirection, Vector2 playerVelocity) {
        if (canShootExplosion) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(explosionPreFab, spawnpoint.position, Quaternion.identity);
                projectile.Construct(lifeTime: 0.3f, damage: 4f * damagesMultiplier, wasShootBy: ProjectileFrom.Player, slowEffect: slowEffect, isPiercingShots: true);

                Rigidbody2D projectileRigidBody = projectile.GetRigidbody2D();
                projectileRigidBody.velocity = playerVelocity; // Same velocity as player
            }
            
            StopCoroutine(shotCoroutine);
            canShoot = false;
            canShootExplosion = false;
            shotCoroutine = StartCoroutine(ShotDelay());
            StartCoroutine(ShotExplosionDelay());
        }
    }

    private IEnumerator ShotDelay() {
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }

    private IEnumerator ShotLaserDelay() {
        yield return new WaitForSeconds(3f);
        canShootLaser = true;
    }

    private IEnumerator ShotBreathDelay() {
        yield return new WaitForSeconds(5f);
        canShootBreath = true;
    }

    private IEnumerator ShotExplosionDelay() {
        yield return new WaitForSeconds(4f);
        canShootExplosion = true;
    }

    public void Spell(Vector2 spellDirection, SpellType spellType, Vector2 playerVelocity) {
        switch (spellType) {
            case SpellType.Laser:
                ShotLaser(spellDirection, playerVelocity);
                break;
            case SpellType.Breath:
                ShotBreath(spellDirection, playerVelocity);
                break;
            case SpellType.Explosion:
                ShotExplosion(spellDirection, playerVelocity);
                break;
            case SpellType.None:
                break;
        }
    }
}
