using System;
using UnityEngine;

public class GameManager : ISingleton<GameManager> {

    public EventHandler<OnScoreChangedArgs> OnScoreChanged;
    public class OnScoreChangedArgs : EventArgs {
        public int score;
    }
    
    private int score = 0;

    private void Update() {
        if (IsGameOver()) {
            return;
        }

    }

    private void SpawnEnemy() {

    }


    public bool IsGameOver() {
        return false; //Player.Instance.IsDead();
    }

    private void Enemy_OnEnemyDestroy(Enemy sender, Enemy.OnEnemyDestroyArgs args) {
        if (IsGameOver()) {
            return;
        }



        // Obstacle obstacle = sender as Obstacle;
        // obstacleSpawnedList.Remove(obstacle);

        // Destroy(obstacle.gameObject);

        // AddPointsToScore((int)e.life);

        // SpawnObstacles();
    }

    private void AddPointsToScore(int points) {
        score += points;
        OnScoreChanged?.Invoke(this, new OnScoreChangedArgs { score = this.score });
    }
}
