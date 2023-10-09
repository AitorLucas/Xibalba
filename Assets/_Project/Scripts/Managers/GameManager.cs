using System;
using System.Collections;
using UnityEngine;

public class GameManager : ISingleton<GameManager> {

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private LevelEnemiesSO[] levelEnemiesSOArray;

    public EventHandler<OnScoreChangedArgs> OnScoreChanged;
    public class OnScoreChangedArgs : EventArgs {
        public int score;
    }
    public EventHandler<OnLevelChangedArgs> OnLevelChanged;
    public class OnLevelChangedArgs: EventArgs {
        public int level;
    }
    
    private int score = 0;
    private int enemiesAlive = 0;
    private int currentLevel = 1;

    private void Start() {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies() {
        new WaitForSeconds(1.5f);

        LevelEnemiesSO currentLevelSO = levelEnemiesSOArray[currentLevel - 1];

        foreach (var enemyDictionary in currentLevelSO.enemiesDictionary) {
            Enemy enemyPrefab = enemyDictionary.Key.enemy;
            int spawnCount = enemyDictionary.Value;

            for (int i = 0; i < spawnCount; i += 1) {
                int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
                Transform spawnPosition = spawnPoints[randomIndex];
                
                Enemy enemy = Instantiate<Enemy>(enemyPrefab, spawnPosition.position, Quaternion.identity);
                enemy.OnEnemyDestroy += Enemy_OnEnemyDestroy;
                enemiesAlive += 1;
            }    
        }

        yield return null;
    }

    private void Enemy_OnEnemyDestroy(object sender, Enemy.OnEnemyDestroyArgs args) {
        if (IsGameOver()) {
            return;
        }

        AddPointsToScore((int)args.experience);
        enemiesAlive -= 1;
        Player.Instance.HealForKill();

        if (enemiesAlive <= 0) {
            ChangeLevel();
        }
    }

    private void AddPointsToScore(int points) {
        score += points;
        OnScoreChanged?.Invoke(this, new OnScoreChangedArgs { score = this.score });
    }

    private void ChangeLevel() {
        currentLevel += 1;
        OnLevelChanged?.Invoke(this, new OnLevelChangedArgs { level = this.currentLevel });
        StartCoroutine(SpawnEnemies());
        StartImprovementsSelection();
    }

    private void StartImprovementsSelection() {
        ImprovementManager.Instance.StartImprovementsSelection();
    }

    private bool IsGameOver() {
        return Player.Instance.IsDead();
    }

    public void Restart_DEBUG() {
        currentLevel = 1;
        StartCoroutine(SpawnEnemies());
    }

    public void StartImprovementsSelection_DEBUG() {
        StartImprovementsSelection();
    }
}
