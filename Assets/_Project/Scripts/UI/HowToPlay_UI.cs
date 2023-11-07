using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay_UI : MonoBehaviour {
    
    [SerializeField] private Button closeButton;

    private void Start() {
        gameObject.SetActive(false);

        closeButton.onClick.AddListener(() => {
           gameObject.SetActive(false);
        });
    }

    public void Show() {
        gameObject.SetActive(true);
    }
}