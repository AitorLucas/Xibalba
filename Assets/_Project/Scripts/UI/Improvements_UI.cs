using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Improvements_UI : MonoBehaviour {

    // [SerializeField] private TextMeshProUGUI improvement1Title;
    // [SerializeField] private TextMeshProUGUI improvement2Title;
    // [SerializeField] private TextMeshProUGUI improvement3Title;
    [SerializeField] private TextMeshProUGUI improvement1Description;
    [SerializeField] private TextMeshProUGUI improvement2Description;
    [SerializeField] private TextMeshProUGUI improvement3Description;
    [SerializeField] private Button improvement1Button;
    [SerializeField] private Button improvement2Button;
    [SerializeField] private Button improvement3Button;

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
        improvement1Description.text = improvementSOsList[0].name;
        improvement2Description.text = improvementSOsList[1].name;
        improvement3Description.text = improvementSOsList[2].name;
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