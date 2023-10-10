using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD_UI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image lifeBarImage;

    private float timerCount;

    private void Start() {
        Player.Instance.OnPlayerLifeChanged += Player_OnPlayerLifeChanged;

        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
        GameManager.Instance.OnLevelChanged += GameManager_OnLevelChanged;
    }

    private void FixedUpdate() {
        timerCount += Time.fixedDeltaTime;
        timeText.text = "Time: " + (int)timerCount;
    }

    private void Player_OnPlayerLifeChanged(object sender, Player.OnPlayerLifeChangedArgs args) {
        lifeBarImage.fillAmount = args.currentLifeNormalized;
    }

    private void GameManager_OnScoreChanged(object sender, GameManager.OnScoreChangedArgs args) {
        scoreText.text = "Score: " + (int)args.score;
    }

    private void GameManager_OnLevelChanged(object sender, GameManager.OnLevelChangedArgs args) {
        levelText.text = "Level: " + args.level;
    }
}