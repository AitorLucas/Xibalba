using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour {

    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;

    private Rigidbody2D playerRigidbody;
    private Animator animator;

    private Vector3 maxVelocity = Vector3.one;
    private bool isFacingRight = false;

    private void Awake() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Move(Vector2 movement, float speed) {
        // TODO: ADD SMOOTHNESS
        // playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, moveVelocity, ref maxVelocity, movementSmoothing);
        // playerRigidbody.MovePosition(playerRigidbody.position + movement * speed);
        float kMagicNumber = 30f;
        playerRigidbody.velocity = movement * speed * kMagicNumber;
       
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
        playerRigidbody.velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }

    private void FlipHorizontal() {
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }
}
