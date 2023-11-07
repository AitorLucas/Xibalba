using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour {
    
    // - Constructed
    private Transform[] spawnPoints;
    private ProjectileSO shotProjectileSO;
    private float shotSpeed;

    private bool canShoot = true;

    public void Construct(Transform[] spawnPoints, ProjectileSO shotProjectileSO, float shotSpeed) {
        this.spawnPoints = spawnPoints;
        this.shotProjectileSO = shotProjectileSO;
        this.shotSpeed = shotSpeed;
    }

    public void Shot(Vector2 shotDirection, Vector2 enemyVelocity) {        
        if (canShoot) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(shotProjectileSO.projectile, spawnpoint.position, Quaternion.identity);
                projectile.Construct(lifeTime: shotProjectileSO.lifeTime, damage: shotProjectileSO.damage, wasShootBy: ProjectileFrom.Enemy, slowEffect: 0, piercingShots: 0, isSpell: false);
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
        yield return new WaitForSeconds(shotProjectileSO.shotDelay);
        canShoot = true;
    }
}
