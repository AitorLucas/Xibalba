using System;
using System.Collections;
using UnityEngine;

public class PlayerShot : MonoBehaviour {

    // - Constructed
    private Transform[] spawnPoints;
    private ProjectileSO shotProjectileSO;
    private ProjectileSO laserProjectileSO;
    private ProjectileSO breathProjectileSO;
    private ProjectileSO explosionProjectileSO;
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

    public void Construct(Transform[] spawnPoints, ProjectileSO shotProjectileSO, ProjectileSO laserProjectileSO, ProjectileSO breathProjectileSO, ProjectileSO explosionProjectileSO, float shotSpeed, float slowEffect, float explosionRangeMultiplier, float extrasShots, float damagesMultiplier, bool isPiercingShots, bool isExplosionWithFireTrails, bool isLaserWithGlobalRange, bool isExplosionPositionFree) {
        this.spawnPoints = spawnPoints;
        this.shotProjectileSO = shotProjectileSO;
        this.laserProjectileSO = laserProjectileSO;
        this.breathProjectileSO = breathProjectileSO;
        this.explosionProjectileSO = explosionProjectileSO;
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

    public void ShotBurst(Vector2 shotDirection, Vector2 playerVelocity) {
        if (canShoot) {
            StartCoroutine(ShootBurst(shotDirection: shotDirection, playerVelocity: playerVelocity));

            if (shotCoroutine != null) {
                StopCoroutine(shotCoroutine);
            }
            canShoot = false;
            shotCoroutine = StartCoroutine(ShotDelay());
        }
    }

    private IEnumerator ShootBurst(Vector2 shotDirection, Vector2 playerVelocity) {
        for (int i = 0; i < 1 + extrasShots; i++) {
            Shoot(shotDirection: shotDirection, playerVelocity: playerVelocity);
            yield return new WaitForSeconds(0.1f); // Burst delay
        }
    }

    void Shoot(Vector2 shotDirection, Vector2 playerVelocity) {
        foreach (var spawnpoint in spawnPoints) {
            Projectile projectile = Instantiate(shotProjectileSO.projectile, spawnpoint.position, Quaternion.Euler(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg) * Quaternion.Euler(shotProjectileSO.projectile.transform.rotation.eulerAngles));
            projectile.Construct(lifeTime: 1f, damage: 1f * damagesMultiplier, wasShootBy: ProjectileFrom.Player, slowEffect: slowEffect, isPiercingShots: isPiercingShots, isSpell: false);

            Rigidbody2D projectileRigidBody = projectile.GetRigidbody2D();

            projectileRigidBody.velocity = playerVelocity / 3; // Adds a bit of starting speed according to player movement
            
            float kMagicNumber = 10f;
            projectileRigidBody.AddForce(shotDirection * shotSpeed * kMagicNumber, ForceMode2D.Impulse);
        }
    }

    private void ShotLaser(Vector2 shotDirection) {
        if (canShootLaser) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(laserProjectileSO.projectile, (Vector2)spawnpoint.position + (shotDirection * 0.6f), Quaternion.Euler(new Vector3(0, 0, -90)));
                projectile.Construct(lifeTime: laserProjectileSO.lifeTime, damage: laserProjectileSO.damage * damagesMultiplier, wasShootBy: ProjectileFrom.Player, slowEffect: slowEffect, isPiercingShots: true, isSpell: true);
                projectile.transform.Rotate(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg);

                if (isLaserWithGlobalRange) {
                    projectile.transform.localScale = new Vector3(2f, 0.3f, 0.3f);
                }
            }
            
            StopCoroutine(shotCoroutine);
            canShoot = false;
            canShootLaser = false;
            shotCoroutine = StartCoroutine(ShotDelay());
            StartCoroutine(ShotLaserDelay());
        }
    }

    private void ShotBreath(Vector2 shotDirection) {
        if (canShootBreath) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(breathProjectileSO.projectile, (Vector2)spawnpoint.position + (shotDirection * 0.6f), Quaternion.Euler(new Vector3(0, 0, -90)));
                projectile.Construct(lifeTime: breathProjectileSO.lifeTime, damage: breathProjectileSO.damage * damagesMultiplier, wasShootBy: ProjectileFrom.Player, slowEffect: slowEffect, isPiercingShots: true, isSpell: true);
                projectile.transform.Rotate(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg);
            }
            
            StopCoroutine(shotCoroutine);
            canShoot = false;
            canShootBreath = false;
            shotCoroutine = StartCoroutine(ShotDelay());
            StartCoroutine(ShotBreathDelay());

            Player.Instance.ActivateShield();
        }
    }

    private void ShotExplosion(Vector2 shotDirection) {
        if (canShootExplosion) {
            foreach (var spawnpoint in spawnPoints) { // TODO: THIS DONT MAKE SENSE (REFACTOR TO ONLY ONE SPAWNPOINT)
                
                Vector2 spawnPosition = spawnpoint.position;
                if (isExplosionPositionFree) {
                    Vector2 mousePosition = InputManager.Instance.GetShotDirectionVector();
                    Vector2 mouseInWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));

                    spawnPosition = mouseInWorldPosition;
                }

                Projectile projectile = Instantiate(explosionProjectileSO.projectile, spawnPosition, Quaternion.identity);
                projectile.Construct(lifeTime: explosionProjectileSO.lifeTime, damage: explosionProjectileSO.damage * damagesMultiplier, wasShootBy: ProjectileFrom.Player, slowEffect: slowEffect, isPiercingShots: true, isSpell: true);
                projectile.transform.localScale *= explosionRangeMultiplier;
            }
            
            StopCoroutine(shotCoroutine);
            canShoot = false;
            canShootExplosion = false;
            shotCoroutine = StartCoroutine(ShotDelay());
            StartCoroutine(ShotExplosionDelay());
        }
    }

    private IEnumerator ShotDelay() {
        yield return new WaitForSeconds(shotProjectileSO.shotDelay);
        canShoot = true;
    }

    private IEnumerator ShotLaserDelay() {
        yield return new WaitForSeconds(laserProjectileSO.shotDelay);
        canShootLaser = true;
    }

    private IEnumerator ShotBreathDelay() {
        yield return new WaitForSeconds(breathProjectileSO.shotDelay);
        canShootBreath = true;
    }

    private IEnumerator ShotExplosionDelay() {
        yield return new WaitForSeconds(explosionProjectileSO.shotDelay); 
        canShootExplosion = true;
    }

    public void Spell(Vector2 spellDirection, SpellType spellType) {
        switch (spellType) {
            case SpellType.Laser:
                ShotLaser(spellDirection);
                break;
            case SpellType.Breath:
                ShotBreath(spellDirection);
                break;
            case SpellType.Explosion:
                ShotExplosion(spellDirection);
                break;
            case SpellType.None:
                break;
        }
    }
}
