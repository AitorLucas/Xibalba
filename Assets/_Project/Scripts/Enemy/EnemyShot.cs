using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour {
    
    // - Constructed
    private Transform[] spawnPoints;
    private Projectile projectilePreFab;
    private float shotDelay;
    private float shotSpeed;

    private bool canShoot = true;

    public void Construct(Transform[] spawnPoints, Projectile projectilePreFab, float shotDelay, float shotSpeed) {
        this.spawnPoints = spawnPoints;
        this.projectilePreFab = projectilePreFab;
        this.shotDelay = shotDelay;
        this.shotSpeed = shotSpeed;
    }

    public void Shot(Vector2 shotDirection, Vector2 enemyVelocity) {        
        if (canShoot) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(projectilePreFab, spawnpoint.position, Quaternion.identity);
                projectile.Construct(lifeTime: 1f, damage: 1f, wasShootBy: ProjectileFrom.Enemy, slowEffect: 0, isPiercingShots: false);
                projectile.transform.Rotate(new Vector3 (0, 0, Mathf.Atan2(-shotDirection.x, shotDirection.y)) * Mathf.Rad2Deg);

                Rigidbody2D projectileRigidBody = projectile.GetComponent<Rigidbody2D>();

                projectileRigidBody.velocity = enemyVelocity / 3; // Adds a bit of starting speed according to player movement
                
                float kMagicNumber = 10f;
                projectileRigidBody.AddForce(shotDirection * shotSpeed * kMagicNumber, ForceMode2D.Impulse);
            }

            canShoot = false;
            StartCoroutine(ShootDelay());
        }
    }

    private IEnumerator ShootDelay() {
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
}
