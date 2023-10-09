using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImprovementManager : ISingleton<ImprovementManager> {

    [SerializeField] Improvements_UI improvements_UI;
    [SerializeField] List<ImprovementSO> improvementsSOList;

    public EventHandler<OnImprovementStartSelectionArgs> OnImprovementStartSelection;
    public class OnImprovementStartSelectionArgs: EventArgs {
        public List<ImprovementSO> improvementSOList;
    }
    public EventHandler<OnImprovementSelectedArgs> OnImprovementSelected;
    public class OnImprovementSelectedArgs: EventArgs {
        public ImprovementSO improvementSO;
        public int count;
    }

    private List<ImprovementSO> possibleImprovements;
    private Dictionary<ImprovementSO, int> playerImprovementsDictionary = new Dictionary<ImprovementSO, int>();
    
    private void Start() {
        improvements_UI.OnImprovementSelected += Improvements_UI_OnImprovementSelected;

        possibleImprovements = improvementsSOList;
    }

    public void StartImprovementsSelection() {
        improvements_UI.StartImprovementsSelection(GetRandomImprovements(count: 3));
    }

    private List<ImprovementSO> GetRandomImprovements(int count) {
        List<ImprovementSO> weightedImprovements = new List<ImprovementSO>();

        foreach(ImprovementSO improvementSO in possibleImprovements) {
            int weight = (int)improvementSO.improvementRarity;

            for (int i = 0; i < weight; i++) {
                weightedImprovements.Add(improvementSO);
            }
        }

        System.Random rand = new System.Random();
        weightedImprovements = weightedImprovements.OrderBy(_ => rand.Next()).Distinct().ToList();

        List<ImprovementSO> randomImprovements = weightedImprovements.Take(count).ToList();

        return randomImprovements;
    }

    private void Improvements_UI_OnImprovementSelected(object sender, Improvements_UI.OnImprovementSelectedArgs args) {
        ImprovementSO improvementSO = args.improvementSO;

        if (playerImprovementsDictionary.ContainsKey(improvementSO)) {
            playerImprovementsDictionary[improvementSO] += 1;
        } else {
            playerImprovementsDictionary[improvementSO] = 1;
        }

        OnImprovementSelected?.Invoke(this, new OnImprovementSelectedArgs { 
            improvementSO = improvementSO,
            count = playerImprovementsDictionary[improvementSO]
        });

        if (improvementSO.improvementRarity == ImprovementRarity.Gold) {
            possibleImprovements.Remove(improvementSO);
        }
    }
}
