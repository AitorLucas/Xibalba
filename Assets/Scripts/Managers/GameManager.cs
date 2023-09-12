using System;
using UnityEngine;

public class GameManager : ISingleton<GameManager> {

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Enemy[] enemies;

    public EventHandler<OnScoreChangedArgs> OnScoreChanged;
    public class OnScoreChangedArgs : EventArgs {
        public int score;
    }
    
    private int score = 0;

    private void Start() {
        SpawnEnemies();
    }

    private void Update() {
        // if (IsGameOver()) {
        //     return;
        // }

    }

    public void SpawnEnemies() {
        for(int i = 0; i < spawnPoints.Length; i++) {
            Enemy enemy = Instantiate<Enemy>(enemies[UnityEngine.Random.Range(0, enemies.Length)], spawnPoints[i].position, Quaternion.identity);
            enemy.OnEnemyDestroy += Enemy_OnEnemyDestroy;
        }
    }

    public bool IsGameOver() {
        return Player.Instance.IsDead();
    }

    private void Enemy_OnEnemyDestroy(object sender, Enemy.OnEnemyDestroyArgs args) {
        if (IsGameOver()) {
            return;
        }

        AddPointsToScore((int)args.experience);
    }

    private void AddPointsToScore(int points) {
        score += points;
        OnScoreChanged?.Invoke(this, new OnScoreChangedArgs { score = this.score });
    }
}
