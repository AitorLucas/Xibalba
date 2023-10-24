using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Improvements_UI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI improvement1Description;
    [SerializeField] private TextMeshProUGUI improvement2Description;
    [SerializeField] private TextMeshProUGUI improvement3Description;
    [SerializeField] private Button improvement1Button;
    [SerializeField] private Button improvement2Button;
    [SerializeField] private Button improvement3Button;
    [SerializeField] private Image improvement1Image;
    [SerializeField] private Image improvement2Image;
    [SerializeField] private Image improvement3Image;

    public EventHandler<OnImprovementSelectedArgs> OnImprovementSelected;
    public class OnImprovementSelectedArgs: EventArgs {
        public ImprovementSO improvementSO;
    }

    private List<ImprovementSO> improvementSOsList;
    private bool isHidden = true;

    private void Start() {
        gameObject.SetActive(!isHidden);

        improvement1Button.onClick.AddListener(() => {
            OnImprovementSelected?.Invoke(this, new OnImprovementSelectedArgs { improvementSO = improvementSOsList[0] });
            ToggleVisibility();
        });
        improvement2Button.onClick.AddListener(() => {
            OnImprovementSelected?.Invoke(this, new OnImprovementSelectedArgs { improvementSO = improvementSOsList[1] });
            ToggleVisibility();
        });
        improvement3Button.onClick.AddListener(() => {
            OnImprovementSelected?.Invoke(this, new OnImprovementSelectedArgs { improvementSO = improvementSOsList[2] });
            ToggleVisibility();
        });
    }

    public void StartImprovementsSelection(List<ImprovementSO> improvementSOs) {
        ToggleVisibility();

        improvementSOsList = improvementSOs;
        UpdateScreenData();
    }

    private void UpdateScreenData() {
        improvement1Description.text = improvementSOsList[0].description;
        improvement2Description.text = improvementSOsList[1].description;
        improvement3Description.text = improvementSOsList[2].description;

        improvement1Image.sprite = improvementSOsList[0].sprite;
        improvement2Image.sprite = improvementSOsList[1].sprite;
        improvement3Image.sprite = improvementSOsList[2].sprite; 
    }

    private void ToggleVisibility() {
        if (Player.Instance.IsDead()) {
            return;
        }

        isHidden = !isHidden;
        
        if (isHidden) {
            GameManager.Instance.UnpauseGame();
        } else {
            GameManager.Instance.PauseGame();
        }

        gameObject.SetActive(!isHidden);
    }
}