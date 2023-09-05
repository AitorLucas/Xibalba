using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour {

     [Range(1, 3f)] [SerializeField] private float lifeTime = 1f;

    private void Start() {
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay() {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider) {  
        if (collider.transform.TryGetComponent(out Enemy enemy)) {
            enemy.Kill();
        }
    }
}
