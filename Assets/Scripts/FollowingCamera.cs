using UnityEngine;

public class FollowingCamera : MonoBehaviour {
    
    [SerializeField] private float lerpSpeed = 1.0f;
    private Player player;

    private void Start() {

        player = Player.Instance;

        // offset = transform.position - player.transform.position; 
        // Needed if player wont be in center
    }

    private void Update() {
        Vector3 newPosition = new Vector3(
            Mathf.Clamp(player.transform.position.x, -4f, 4f),
            Mathf.Clamp(player.transform.position.y, -2.5f, 2f),
            transform.position.z
        );
        transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
    }
}
