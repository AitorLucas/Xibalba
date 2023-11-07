using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_UI : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private TextMeshProUGUI additionalText;
    [SerializeField] private Button menuButton;

    private int level = 0;
    private int time = 0;

    private void Start() {
        gameObject.SetActive(false);

        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        
        GameManager.Instance.OnTimeEnded += GameManager_OnTimeEnded;
        GameManager.Instance.OnGameWon += GameManager_OnGameWon;


        menuButton.onClick.AddListener(() => {
            GameManager.Instance.UnpauseGameTime();
            Loader.Load(Loader.Scene.InitialScene);
        });
    }

    private void Player_OnPlayerDeath(object sender, EventArgs args) {
        GameManager.Instance.PauseGameTime();
        gameObject.SetActive(true);

        titleText.text = "GAME\nOVER";
        subtitleText.text = "Você morreu!";
        additionalText.text = "Level: " + GameManager.Instance.GetLevel().ToString();
    }

    private void GameManager_OnTimeEnded(object sender, EventArgs args) {
        GameManager.Instance.PauseGameTime();
        gameObject.SetActive(true);

        titleText.text = "GAME\nOVER";
        subtitleText.text = "O tempo acabou, Xibalba te consumiu. Você perdeu!";
        additionalText.text = "Level: " + GameManager.Instance.GetLevel().ToString();
    }

    private void GameManager_OnGameWon(object sender, EventArgs args) {
        GameManager.Instance.PauseGameTime();
        gameObject.SetActive(true);

        titleText.text = "THE END";
        subtitleText.text = "Você se tornou invencível e ascendeu como uma divindade, expurgando Xibalba.";
        additionalText.text = "Seu tempo foi de " + GameManager.Instance.GetTime().ToString() + " segundos, parabéns!";
    }

}