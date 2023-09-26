using UnityEngine;

public class Aim : MonoBehaviour {

    private bool isClipped = true;

    private void Update() {
        Vector2 mousePosition = InputManager.Instance.GetShotDirectionVector();
        Vector2 mouseInWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        Vector2 playerPosition = Player.Instance.transform.position;

        Vector2 playerToMouse = mouseInWorldPosition - playerPosition;

        if (isClipped) { 
            transform.position = playerPosition + (playerToMouse.normalized * 0.7f);
        } else {
            transform.position = mouseInWorldPosition;
        }

        Quaternion toRatation = Quaternion.LookRotation(Vector3.forward, playerToMouse);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRatation, 360);
    }
}
