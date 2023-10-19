using UnityEngine;

public class FollowingCamera : MonoBehaviour {
    
    [Range(0, 1f)][SerializeField] private float lerpSpeed = 0.1f;
    [SerializeField] private float xLeftLimit = -3f;
    [SerializeField] private float xRightLimit = 3f;
    [SerializeField] private float yBottomLimit = -2.1f;
    [SerializeField] private float yTopLimit = 2f;
    [SerializeField] private float tolerance = 0.1f;
    private Player player;

    private void Start() {

        player = Player.Instance;

        // offset = transform.position - player.transform.position; 
        // Needed if player wont be in center
    }

    private void Update() {
        Vector3 newPosition = new Vector3(
            Mathf.Clamp(player.transform.position.x, (1 + tolerance) * xLeftLimit , (1 + tolerance) * xRightLimit),
            Mathf.Clamp(player.transform.position.y, (1 + tolerance) * yBottomLimit, (1 + tolerance) * yTopLimit),
            transform.position.z
        );
        transform.position = Vector3.Lerp(transform.position, newPosition, lerpSpeed);
    }
}
