using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
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
    public EventHandler<OnTimeProgressChangedArgs> OnTimeProgressChanged;
    public class OnTimeProgressChangedArgs : EventArgs {
        public float timeNormalized;
    }
    public EventHandler<EventArgs> OnTimeEnded;
    public EventHandler<EventArgs> OnGameWon;
    public EventHandler<OnGamePauseChangedArgs> OnGamePauseChanged;
    public class OnGamePauseChangedArgs: EventArgs {
        public bool isPaused;
    }

    private bool isPaused = false;
    private bool isTimePaused = false;
    private int score = 0;
    private int enemiesAlive = 0;
    private int currentLevelCount = 1;
    private LevelEnemiesSO currentLevelSO;
    private float timerCount;
    private float maxTime = 60 * 10; // 10 minutes

    private void Start() {
        StartCoroutine(SpawnEnemies());
        // StartCoroutine(InitialAnimation());

        InputManager.Instance.OnGeneralPauseGameAction += InputManager_OnGeneralPauseGameAction;

        ImprovementManager.Instance.OnImprovementSelected += ImprovementManager_OnImprovementSelected;
    }

    private void FixedUpdate() {
        timerCount += Time.fixedDeltaTime;

        OnTimeProgressChanged?.Invoke(this, new OnTimeProgressChangedArgs { timeNormalized = timerCount/maxTime });

        if (timerCount > maxTime) {
            OnTimeEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    // private IEnumerator InitialAnimation() {
    //     // // PauseGame();

    //     // float startTime = Time.unscaledTime;

    //     // Player player = Player.Instance;
    //     // while ((Vector2)player.transform.position != Vector2.zero && (Time.unscaledTime - startTime) < 3f) {
    //     //     Debug.Log("Aqui");
    //     //     player.Move(Vector2.down);
    //     //     // yield return null;
    //     // }
    //     yield return null;
    //     // // UnpauseGame();
    // }

    private IEnumerator SpawnEnemies() {
        currentLevelSO = levelEnemiesSOArray[Math.Min(currentLevelCount - 1, levelEnemiesSOArray.Length - 1)];
        List<Transform> possibleSpawnPoints = GetSpawnPointsFarFromPlayer();
        List<Transform> availableSpawnPoints = possibleSpawnPoints; 

        enemiesAlive = currentLevelSO.GetEnemiesCount();
        // OnLevelProgressChanged?.Invoke(this, new OnLevelProgressChangedArgs { currentLevelNormalized = 0 });

        foreach (var enemyDictionary in currentLevelSO.enemiesDictionary) {
            Enemy enemyPrefab = enemyDictionary.Key.enemy;
            int spawnCount = enemyDictionary.Value;

            for (int i = 0; i < spawnCount; i += 1) {
                int randomIndex = UnityEngine.Random.Range(0, possibleSpawnPoints.Count);

                if (availableSpawnPoints.Count <= 0) {
                    availableSpawnPoints = possibleSpawnPoints;
                }
                Transform spawnPosition = availableSpawnPoints[randomIndex];
                availableSpawnPoints.RemoveAt(randomIndex);
                
                Enemy enemy = Instantiate(enemyPrefab, spawnPosition.position, Quaternion.identity);
                enemy.OnEnemyDestroy += Enemy_OnEnemyDestroy;
            }
        }

        yield return null;
    }  

    private List<Transform> GetSpawnPointsFarFromPlayer() {
        List<Transform> spawnList = new List<Transform>();

        float minDistance = 4f;
        for (int i = 1; i < spawnPoints.Length; i += 1) {
            float distance = Vector3.Distance(Player.Instance.transform.position, spawnPoints[i].position);
            if (distance > minDistance) {
                spawnList.Add(spawnPoints[i]);
            }
        }

        return spawnList;
    }

    private void Enemy_OnEnemyDestroy(object sender, Enemy.OnEnemyDestroyArgs args) {
        if (IsGameOver()) {
            return;
        }

        AddPointsToScore((int)args.experience);

        enemiesAlive -= 1;
        // OnLevelProgressChanged?.Invoke(this, new OnLevelProgressChangedArgs { currentLevelNormalized = 1 - (float)enemiesAlive/currentLevelSO.GetEnemiesCount() });

        Player.Instance.HealForKill();

        if (enemiesAlive <= 0) {
            Invoke("ChangeLevel", 1f);
        }
    }

    private void InputManager_OnGeneralPauseGameAction(object sender, EventArgs args) {
        if (IsGameOver()) {
            return;
        }

        isPaused = !isPaused;

        if (isPaused) {
            PauseGame();
        } else {
            UnpauseGame();
        }
    }

    private void ImprovementManager_OnImprovementSelected(object sender, ImprovementManager.OnImprovementSelectedArgs args) {
        if (args.improvementSO.improvementType == ImprovementType.Ascension) {
            OnGameWon?.Invoke(this, EventArgs.Empty);
        } else {
            Player.Instance.AddImprovement(improvementSO: args.improvementSO, count: args.count);
        }
    }

    private void AddPointsToScore(int points) {
        score += points;
        OnScoreChanged?.Invoke(this, new OnScoreChangedArgs { score = this.score });
    }

    private void ChangeLevel() { 
        currentLevelCount += 1;
        OnLevelChanged?.Invoke(this, new OnLevelChangedArgs { level = this.currentLevelCount });
        StartCoroutine(SpawnEnemies());
        if (currentLevelCount > levelEnemiesSOArray.Length) {
            StartLastImprovementSelection();
        } else {
            StartImprovementsSelection();
        }
    }

    private void StartImprovementsSelection() {
        ImprovementManager.Instance.StartImprovementsSelection();
    }

    private void StartLastImprovementSelection() {
        ImprovementManager.Instance.StartLastImprovementSelection();
    }

    public bool IsGameOver() {
        return Player.Instance.IsDead() || timerCount > maxTime;
    }

    public void PauseGameTime() {
        Time.timeScale = 0;
        isTimePaused = true;
    }

    public void UnpauseGameTime() {
        Time.timeScale = 1;
        isTimePaused = false;
    }

    public void PauseGame() {
        PauseGameTime();
        OnGamePauseChanged?.Invoke(this, new OnGamePauseChangedArgs { isPaused = true });
    }

    public void UnpauseGame() {
        UnpauseGameTime();
        OnGamePauseChanged?.Invoke(this, new OnGamePauseChangedArgs { isPaused = false });
    }

    public bool IsTimePaused() {
        return isTimePaused;
    }

    public int GetLevel() {
        return currentLevelCount;
    }

    public int GetTime() {
        return (int)timerCount;
    }

    public void Restart_DEBUG() {
        currentLevelCount = 1;
        StartCoroutine(SpawnEnemies());
    }

    public void StartImprovementsSelection_DEBUG() {
        StartImprovementsSelection();
    }

    public void StartLastImprovementSelection_DEBUG() {
        StartLastImprovementSelection();
    }
}
