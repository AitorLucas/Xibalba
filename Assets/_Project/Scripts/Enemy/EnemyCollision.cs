using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EnemyController))]
public class EnemyCollision : MonoBehaviour {

    private Collider2D collider;
    private EnemyController enemyController;

    private float damageCollision;

    public void Construct(float damageCollision) {
        this.damageCollision = damageCollision;
    }

    private void Awake() {
        enemyController = GetComponent<EnemyController>();
    }

    private void Start() {
        collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.TryGetComponent(out Player player)) {
            player.Hurt(damageCollision);
        } else if (collision.transform.TryGetComponent(out Obstacle obstacle)) {
            enemyController.InvertMovingDirection();
        }
    }

    public void DisableCollision() {
        collider.enabled = false;
    }
}
