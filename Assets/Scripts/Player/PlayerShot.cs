using System.Collections;
using UnityEngine;

public class PlayerShot : MonoBehaviour {

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Projectile projectilePreFab;
    [Range(0.1f, 3.0f)][SerializeField] private float shootDelay = 1.0f;

    private bool canShoot = true;

    public void Shoot(Vector2 shot, Vector2 playerVelocity, float speed) {
        if (canShoot) {
            foreach (var spawnpoint in spawnPoints) {
                Projectile projectile = Instantiate(projectilePreFab, spawnpoint.position, Quaternion.identity);
                projectile.transform.Rotate(new Vector3 (0, 0, Mathf.Atan2(-shot.x, shot.y)) * Mathf.Rad2Deg);

                Rigidbody2D projectileRigidBody = projectile.GetComponent<Rigidbody2D>();

                projectileRigidBody.velocity = playerVelocity / 1.5f; // Adds a bit of starting speed according to player movement
                
                float kMagicNumber = 10f;
                projectileRigidBody.AddForce(shot * speed * kMagicNumber, ForceMode2D.Impulse);
            }

            canShoot = false;
            StartCoroutine(ShootDelay());
        }
    }

    private IEnumerator ShootDelay() {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
