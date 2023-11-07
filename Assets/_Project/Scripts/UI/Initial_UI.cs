using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Initial_UI : MonoBehaviour {
    
    [SerializeField] private Button startButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private HowToPlay_UI howToPlay_UI;

    private void Start() {
        startButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });

        controlsButton.onClick.AddListener(() => {
            howToPlay_UI.Show();
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}