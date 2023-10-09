using UnityEngine;

public class Obstacle : MonoBehaviour {

    public void AddKnockback(Vector2 direction) {
        if (TryGetComponent(out Rigidbody2D rigidbody2D)) {
            rigidbody2D.AddForce(direction * 10f, ForceMode2D.Impulse);
        }
    }
}
