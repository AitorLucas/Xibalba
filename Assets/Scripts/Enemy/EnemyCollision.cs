using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyCollision : MonoBehaviour {

    public float damagePerHit = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.TryGetComponent(out Player player)) {
            player.Hurt(damagePerHit);
        }
    }

}
