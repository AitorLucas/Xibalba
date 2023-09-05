using System.Collections;
using UnityEngine;

public enum EnemyState {
    Wander,
    Follow,
    Die,
};

public class EnemyController : MonoBehaviour {

    private GameObject player;
    public EnemyState currentState = EnemyState.Wander;
    public float range;
    public float speed;
    private bool chooseDirection = false;
    private bool isDead = false;
    private Vector3 randomDirection;

    private void Start() {
        player = Player.Instance.gameObject;
    }

    private void Update() {
        switch(currentState) {
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):
                break;
        }

        if (currentState != EnemyState.Die) {
            if (IsPlayerInRange(range)) {
                currentState = EnemyState.Follow;
            } else  {
                currentState = EnemyState.Wander;
            }
        }
        
    }

    private bool IsPlayerInRange(float range) {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private void Wander() { 
        if (!chooseDirection) {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed* Time.deltaTime;
        if (IsPlayerInRange(range)) {
            currentState = EnemyState.Follow;
        }
    }

    private IEnumerator ChooseDirection() {
        chooseDirection = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        randomDirection = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDirection);
        // transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDirection = false;
    }

    private void Follow() {
       transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
