using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debug_UI : MonoBehaviour {
    
    [SerializeField] private Button respawnButton;
    [SerializeField] private Button godModeButton;
    [SerializeField] private Button improvementsButton;

    private bool isHidden = true;

    private void Start() {
        gameObject.SetActive(!isHidden);

        InputManager.Instance.OnDebugTogglePaneAction += InputManager_OnDebugTogglePaneAction;

        respawnButton.onClick.AddListener(() => {
            GameManager.Instance.Restart_DEBUG();
        });
        godModeButton.onClick.AddListener(() => {
            Player.Instance.ToggleGodMode_DEBUG();
        });
        improvementsButton.onClick.AddListener(() => {
            GameManager.Instance.StartImprovementsSelection_DEBUG();
        });
    }

    private void OnGUI() {
        if (isHidden) {
            return;
        }

        GUILayout.BeginArea(new Rect(20, 20, 300, 700));
        GUILayout.Label("--------- Player ---------" + "\n" + Player.Instance.GetPlayerData_DEBUG());
        GUILayout.Label("------ Improvements ------" + "\n" + Player.Instance.GetImprovementsData_DEBUG());
        GUILayout.EndArea();
    }
    
    private void InputManager_OnDebugTogglePaneAction(object sender, EventArgs args) {
        isHidden = !isHidden;
        gameObject.SetActive(!isHidden);
    }    
}