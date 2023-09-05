using System;
using UnityEngine;

public class GameManager : ISingleton<GameManager> {

    // [SerializeField] private ObstacleSO[] obstacleSOArray;
    // [SerializeField] private PowerUpSO[] powerUpSOArray;

    // public EventHandler<OnScoreChangedArgs> OnScoreChanged;
    // public class OnScoreChangedArgs : EventArgs {
    //     public int score;
    // }

    // private List<Obstacle> obstacleSpawnedList = new List<Obstacle>();
    // private PowerUp powerUpSpawned = null;

    // private int score = 0;
    // private float minRenderDistance = 30f;
    // private float maxRenderDistance = 50f;
    // private int maxObstacleSpawned = 60;
    // private float powerUpSpawnInterval = 10f;
    // private float powerUpTime = 0f;
    // public bool isGameOver = false;

    // private Player player;

    // private void Start() {
    //     player = Player.Instance;

    //     Vector3 playerPosition = player.transform.position;
    //     SpawnObstacles();

    //     for (int index = 0; index < GetObstacleSpawned(); index ++) {
    //         Obstacle obstacle = obstacleSpawnedList[index];
            
    //         obstacle.ApplyForce();
    //         obstacle.ApplyTorque();
    //     }

    //     // - To start game with a PowerUp spawning
    //     powerUpTime = powerUpSpawnInterval;
    // }

    // private void Update() {
    //     if (IsGameOver()) {
    //         return;
    //     }

    //     RemoveFarObstacles();
    //     SpawnPowerUp();
    // }

    // private void SpawnObstacles() {
    //     while(GetObstacleSpawned() < maxObstacleSpawned) {
    //         // - Calc a range around player subtracting a span near it.
    //         Vector3 spawnPosition = GetValidPositionToSpawn();

    //         // - Spawn new Obstacle
    //         ObstacleSO newObstacleSO = obstacleSOArray[UnityEngine.Random.Range(0, obstacleSOArray.Length)];
    //         Obstacle newObstacle = Instantiate<Obstacle>(newObstacleSO.obstacle, spawnPosition, Quaternion.identity);
        
    //         // - Modify scale and life
    //         float scale = newObstacleSO.baseScale * UnityEngine.Random.Range(0.5f, 2f);
    //         newObstacle.transform.localScale = new Vector3(scale, scale, scale);
    //         newObstacle.DefineLife(newObstacleSO.baseLife * scale);

    //         // - Add movement to Obstacle
    //         newObstacle.ApplyForce();
    //         newObstacle.ApplyTorque();

    //         // - Observe Obstacle event
    //         newObstacle.OnObstacleDestroy += Obstacle_OnObstacleDestroy;

    //         // - Add to array to validate distance afterwards
    //         obstacleSpawnedList.Add(newObstacle);        
    //     }
    // }

    // private void SpawnPowerUp() {
    //     powerUpTime += Time.deltaTime;

    //     if (powerUpTime >= powerUpSpawnInterval) {
    //         powerUpTime = 0;

    //         // - Calc a range around player subtracting a span near it.
    //         Vector3 spawnPosition = GetValidPositionToSpawn();

    //         // - Spawn new PowerUp
    //         PowerUpSO newPowerUpSO = powerUpSOArray[UnityEngine.Random.Range(0, obstacleSOArray.Length)];
    //         PowerUp newPowerUp = Instantiate<PowerUp>(newPowerUpSO.powerUp, spawnPosition, Quaternion.identity);

    //         // - Attribute PowerUp type variant
    //         newPowerUp.DefinePowerUpType(newPowerUpSO.powerUpType);

    //         // - Observe PowerUp event
    //         newPowerUp.OnPowerUpDestroy += PowerUp_OnPowerUpDestroy;

    //         // - Add to variable to validate distance afterwards
    //         powerUpSpawned = newPowerUp;
    //     }
    // }

    // private Vector3 GetValidPositionToSpawn() {
    //     Vector3 playerPosition = player.transform.position;

    //     float radius = UnityEngine.Random.Range(minRenderDistance, maxRenderDistance);
    //     float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

    //     Vector3 spawnPosition = playerPosition + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        
    //     return spawnPosition;
    // }

    // private void RemoveFarObstacles() {
    //     Vector3 playerPosition = player.transform.position;

    //     for (int index = 0; index < GetObstacleSpawned(); index ++) {
    //         Obstacle obstacle = obstacleSpawnedList[index];
    //         float distance = Vector3.Distance(obstacle.transform.position, playerPosition);
    //         if (distance > maxRenderDistance) {    
    //             obstacleSpawnedList.RemoveAt(index);
    //             index -= 1;

    //             Destroy(obstacle.gameObject);
    //         }
    //     }

    //     SpawnObstacles();      
    // }

    // public bool IsGameOver() {
    //     return player.isCrashed;
    // }

    // private void Obstacle_OnObstacleDestroy(object sender, Obstacle.OnObstacleDestroyArgs e) {
    //     if (IsGameOver()) {
    //         return;
    //     }

    //     Obstacle obstacle = sender as Obstacle;
    //     obstacleSpawnedList.Remove(obstacle);

    //     Destroy(obstacle.gameObject);

    //     AddPointsToScore((int)e.life);

    //     SpawnObstacles();
    // }

    // private void PowerUp_OnPowerUpDestroy(object sender, EventArgs e) {
    //     if (IsGameOver()) {
    //         return;
    //     }

    //     PowerUp powerUp = sender as PowerUp;
    //     powerUpSpawned = null;

    //     Destroy(powerUp.gameObject);
    // }

    // private void AddPointsToScore(int points) {
    //     score += points;
    //     OnScoreChanged?.Invoke(this, new OnScoreChangedArgs { score = this.score });
    // }

    // private int GetObstacleSpawned() {
    //     return obstacleSpawnedList.Count;
    // }
}
