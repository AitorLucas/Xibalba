using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD_UI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI scoreText;

    private float timerCount;

    private void Start() {
        Player.Instance.OnPlayerLifeChanged += Player_OnPlayerLifeChanged;

        // menuButton.onClick.AddListener(() => {
        //     Loader.Load(Loader.Scene.InitialScene);
        // });
    }

    private void FixedUpdate() {
        timerCount += Time.fixedDeltaTime;
        scoreText.text = "Score: " + (int)timerCount;
    }

    private void Player_OnPlayerLifeChanged(object sender, Player.OnPlayerLifeChangedArgs e) {

    }
}
