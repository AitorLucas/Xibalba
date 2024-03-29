using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour {
    
    // - Constructed
    private float movementSmoothing;
    private float movementSpeed;

    private Rigidbody2D rigidbody;
    private Animator animator;

    private bool isFacingRight = false;

    public void Construct(float movementSmoothing, float movementSpeed) {
        this.movementSmoothing = movementSmoothing;
        this.movementSpeed = movementSpeed;
    }

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 movement) {
        float kMagicNumber = 30f;
        rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, movement * movementSpeed * kMagicNumber, movementSmoothing);

        if (movement.x > 0 && isFacingRight) {
            FlipHorizontal();
        } else if (movement.x < 0 && !isFacingRight) {
            FlipHorizontal();
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetBool("IsMoving", true);
    }

    public void Stop() {
        rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, Vector2.zero, movementSmoothing);
        animator.SetBool("IsMoving", false);
    }

    public void DeathAnimate() {
        animator.SetBool("IsDead", true);
    }

    private void FlipHorizontal() {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

    public void AddKnockback(Vector2 direction) {
        rigidbody.AddForce(direction * 10f, ForceMode2D.Impulse);
    }

    public void AddSlowEffect(float slowEffect) {
        StartCoroutine(SlowDelay(slowEffect));
    }

    private IEnumerator SlowDelay(float slowEffect) {
        movementSpeed /= slowEffect;
        yield return new WaitForSeconds(3f);
        movementSpeed *= slowEffect;
    }
}
