using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_UI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button menuButton;

    private void Start() {
        gameObject.SetActive(false);

        Player.Instance.OnPlayerLifeChanged += Player_OnPlayerLifeChanged;
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;

        menuButton.onClick.AddListener(() => {
            Time.timeScale = 1;
            Loader.Load(Loader.Scene.InitialScene);
        });
    }
    
    private void GameManager_OnScoreChanged(object sender, GameManager.OnScoreChangedArgs args) {
        scoreText.text = "SCORE: " + args.score.ToString();
    }

    private void Player_OnPlayerLifeChanged(object sender, Player.OnPlayerLifeChangedArgs args) {
        if (Player.Instance.IsDead()) {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }
    }
}