using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Initial_UI : MonoBehaviour {
    
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start() {
        startButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}