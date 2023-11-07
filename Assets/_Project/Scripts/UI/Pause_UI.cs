using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause_UI : MonoBehaviour {
    
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private HowToPlay_UI howToPlay_UI;

    private void Start() {
        gameObject.SetActive(false);

        GameManager.Instance.OnGamePauseChanged += GameManager_OnGamePauseChanged;

        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.UnpauseGameTime();
            gameObject.SetActive(false);
        });

        instructionsButton.onClick.AddListener(() => {
            howToPlay_UI.Show();
        });

        menuButton.onClick.AddListener(() => {
            GameManager.Instance.UnpauseGameTime();
            Loader.Load(Loader.Scene.InitialScene);
        });
    }

    private void GameManager_OnGamePauseChanged(object sender, GameManager.OnGamePauseChangedArgs args) {
        if (args.isPaused) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

}