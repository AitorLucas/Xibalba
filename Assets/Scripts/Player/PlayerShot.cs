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

    private bool canShoot = true;

    public void Construct(Transform[] spawnPoints, Projectile projectilePreFab, Projectile laserPreFab, Projectile breathPreFab, Projectile explosionPreFab, float shotDelay, float shotSpeed) {
        this.spawnPoints = spawnPoints;
        this.projectilePreFab = projectilePreFab;
        this.laserPreFab = laserPreFab;
        this.breathPreFab = breathPreFab;
        this.explosionPreFab = explosionPreFab;
        this.shotDelay = shotDelay;
        this.shotSpeed = shotSpeed;
    }

    public void Shot(Vector2 shotDirection, Vector2 playerVelocity) {
        if (canShoot) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(projectilePreFab, spawnpoint.position, Quaternion.Euler(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg));
                projectile.Construct(lifeTime: 1f, damage: 1f, wasShootBy: ProjectileFrom.Player);
                // projectile.transform.Rotate(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg);

                Rigidbody2D projectileRigidBody = projectile.GetRigidbody2D();

                projectileRigidBody.velocity = playerVelocity / 3; // Adds a bit of starting speed according to player movement
                
                float kMagicNumber = 10f;
                projectileRigidBody.AddForce(shotDirection * shotSpeed * kMagicNumber, ForceMode2D.Impulse);
            }

            canShoot = false;
            StartCoroutine(ShotDelay());
        }
    }

    private bool canShootLaser = true;

    public void ShotLaser(Vector2 shotDirection, Vector2 playerVelocity) {
        if (canShootLaser) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(laserPreFab, (Vector2)spawnpoint.position + (shotDirection * 0.6f), Quaternion.Euler(new Vector3(0, 0, -90)));
                projectile.Construct(lifeTime: 0.25f, damage: 3f, wasShootBy: ProjectileFrom.Player);
                projectile.transform.Rotate(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg);

                Rigidbody2D projectileRigidBody = projectile.GetRigidbody2D();

                projectileRigidBody.velocity = playerVelocity; // Adds a bit of starting speed according to player movement
            }
            
            StopAllCoroutines();
            canShoot = false;
            canShootLaser = false;
            StartCoroutine(ShotDelay());
            StartCoroutine(ShotLaserDelay());
        }
    }

    public void ShotBreath(Vector2 shotDirection, Vector2 playerVelocity) {

    }

    public void ShotExplosion(Vector2 shotDirection, Vector2 playerVelocity) {

    }

    private IEnumerator ShotDelay() {
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }

    private IEnumerator ShotLaserDelay() {
        yield return new WaitForSeconds(3f);
        canShootLaser = true;
    }

    public void Spell(Vector2 spellDirection, SpellType spellType, Vector2 playerVelocity) {

    } 
}
