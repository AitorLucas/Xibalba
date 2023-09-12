using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debug_UI : MonoBehaviour {
    
    [SerializeField] private Button respawnButton;

    private bool isHidden = true;

    private void Start() {
        gameObject.SetActive(false);

        InputManager.Instance.OnDebugTogglePaneAction += InputManager_OnDebugTogglePaneAction;

        respawnButton.onClick.AddListener(() => {
            GameManager.Instance.SpawnEnemies();
        });
    }
    
    private void InputManager_OnDebugTogglePaneAction(object sender, EventArgs args) {
        isHidden = !isHidden;
        gameObject.SetActive(!isHidden);
    }
}