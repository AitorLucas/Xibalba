using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause_UI : MonoBehaviour {
    
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button menuButton;

    private void Start() {
        gameObject.SetActive(false);

        GameManager.Instance.OnGamePauseChanged += GameManager_OnGamePauseChanged;

        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.UnpauseGameTime();
            gameObject.SetActive(false);
        });

        instructionsButton.onClick.AddListener(() => {
            // GameManager.Instance.UnpauseGame();
            // Loader.Load(Loader.Scene.InitialScene);
        });

        menuButton.onClick.AddListener(() => {
            GameManager.Instance.UnpauseGameTime();
            Loader.Load(Loader.Scene.InitialScene);
        });
    }

    private void GameManager_OnGamePauseChanged(object sender, GameManager.OnGamePauseChangedArgs args) {
        if (args.isPaused) {
            gameObject.SetActive(true);
            // GetComponent<SpriteRenderer>().enabled = true;
        } else {
            gameObject.SetActive(false);
            // GetComponent<SpriteRenderer>().enabled = false;
        }
    }

}