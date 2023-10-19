using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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
    
    public EventHandler<OnGamePauseChangedArgs> OnGamePauseChanged; // NEVER USED
    public class OnGamePauseChangedArgs: EventArgs {
        public bool isPaused;
    }
    
    private bool isPaused = false;
    private int score = 0;
    private int enemiesAlive = 0;
    private int currentLevel = 1;

    private void Start() {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(InitialAnimation());
    }

    private IEnumerator InitialAnimation() {
        // PauseGame();

        float startTime = Time.unscaledTime;

        Player player = Player.Instance;
        while ((Vector2)player.transform.position != Vector2.zero && (Time.unscaledTime - startTime) < 3f) {
            Debug.Log("Aqui");
            player.Move(Vector2.down);
            // yield return null;
        }
        yield return null;
        // UnpauseGame();
    }

    private IEnumerator SpawnEnemies() {
        new WaitForSeconds(1.5f);

        LevelEnemiesSO currentLevelSO = levelEnemiesSOArray[Math.Min(currentLevel - 1, levelEnemiesSOArray.Length - 1)];

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

    public void PauseGame() {
        Time.timeScale = 0;
        isPaused = true;
        OnGamePauseChanged?.Invoke(this, new OnGamePauseChangedArgs { isPaused = this.isPaused });
    }

    public void UnpauseGame() {
        Time.timeScale = 1;
        isPaused = false;
        OnGamePauseChanged?.Invoke(this, new OnGamePauseChangedArgs { isPaused = this.isPaused });
    }

    public bool IsPaused() {
        return isPaused;
    }

    public void Restart_DEBUG() {
        currentLevel = 1;
        StartCoroutine(SpawnEnemies());
    }

    public void StartImprovementsSelection_DEBUG() {
        StartImprovementsSelection();
    }
}
