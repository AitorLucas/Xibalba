using System.Collections;
using UnityEngine;

public enum ProjectileFrom {
    Enemy,
    Player,
};

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour {

    [Range(1, 3f)][SerializeField] private float lifeTime = 1f;

    private float damage = 2f; // ADD TO CONSTRUCT
    public ProjectileFrom WasShootBy = ProjectileFrom.Player;

    private void Start() {
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay() {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider) {  
        switch (WasShootBy) {
            case (ProjectileFrom.Enemy):
                if (collider.transform.TryGetComponent(out Player player)) {
                    player.Hurt(damage);
                    Destroy(gameObject);
                }
                break;
            case (ProjectileFrom.Player):
                if (collider.transform.TryGetComponent(out Enemy enemy)) {
                    enemy.Hurt(damage);
                    Destroy(gameObject);
                }
                break;
        }

        if (collider.transform.TryGetComponent(out Obstacle obstacle)) {
            Destroy(gameObject);
        }
    }
}
