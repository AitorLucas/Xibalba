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
    [SerializeField] private Button selectionButton;

    public EventHandler<OnImprovementSelectedArgs> OnImprovementSelected;
    public class OnImprovementSelectedArgs: EventArgs {
        public ImprovementSO improvementSO;
    }

    private List<ImprovementSO> improvementSOsList;
    private bool isHidden = true;
    private int selectedImprovementIndex;
    private int kUnselectedIndex = -1;
    
    private void Awake() {
        selectedImprovementIndex = kUnselectedIndex;
    }

    private void Start() {
        gameObject.SetActive(!isHidden);

        improvement1Button.onClick.AddListener(() => {
            if (selectedImprovementIndex == 0) {
                return;
            }
            int oldIndex = selectedImprovementIndex;
            selectedImprovementIndex = 0;
            UpdateImprovementButtons(oldIndex: oldIndex);
        });
        improvement2Button.onClick.AddListener(() => {
            if (selectedImprovementIndex == 1) {
                return;
            }
            int oldIndex = selectedImprovementIndex;
            selectedImprovementIndex = 1;
            UpdateImprovementButtons(oldIndex: oldIndex);
        });
        improvement3Button.onClick.AddListener(() => {
            if (selectedImprovementIndex == 2) {
                return;
            }
            int oldIndex = selectedImprovementIndex;
            selectedImprovementIndex = 2;
            UpdateImprovementButtons(oldIndex: oldIndex);
        });

        selectionButton.onClick.AddListener(() => {
            Debug.Log("THIS:" + selectedImprovementIndex.ToString());
            if (selectedImprovementIndex == kUnselectedIndex) {
                return;
            }
            OnImprovementSelected?.Invoke(this, new OnImprovementSelectedArgs {
                improvementSO = improvementSOsList[selectedImprovementIndex]
            });
            selectedImprovementIndex = kUnselectedIndex;
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

    private void UpdateImprovementButtons(int oldIndex) {
        if (selectedImprovementIndex == 0) {
            improvement1Button.Select();
        } else if (selectedImprovementIndex == 1) {
            improvement2Button.Select();
        } else if (selectedImprovementIndex == 2) {
            improvement3Button.Select();
        }
    }

    private void ToggleVisibility() {
        if (Player.Instance.IsDead()) {
            return;
        }

        isHidden = !isHidden;
        
        if (isHidden) {
            GameManager.Instance.UnpauseGameTime();
        } else {
            GameManager.Instance.PauseGameTime();
        }

        gameObject.SetActive(!isHidden);
    }
}