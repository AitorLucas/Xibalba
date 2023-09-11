using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD_UI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image lifeBarImage;

    // private float timerCount;

    private void Start() {
        Player.Instance.OnPlayerLifeChanged += Player_OnPlayerLifeChanged;

        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
    }

    private void FixedUpdate() {
        // timerCount += Time.fixedDeltaTime;
        // scoreText.text = "Score: " + (int)timerCount;
    }

    private void Player_OnPlayerLifeChanged(object sender, Player.OnPlayerLifeChangedArgs args) {
        lifeBarImage.fillAmount = args.currentLifeNormalized;
    }

    private void GameManager_OnScoreChanged(object sender, GameManager.OnScoreChangedArgs args) {
        scoreText.text = "Score: " + (int)args.score;
    }
}
