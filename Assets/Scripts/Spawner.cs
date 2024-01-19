using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    // Obstacle prefab
    public GameObject obstaclePrefab;

    // Minimum time interval between spawns
    public float minSpawnInterval = 1.0f;

    // Maximum time interval between spawns
    public float maxSpawnInterval = 5.0f;

    // Initial speed of obstacles
    public float obstacleSpeed = 5.0f;

    // Rate at which obstacle speed increases over time
    public float speedIncreaseRate = 0.1f;

    // Probability of obstacle having a slight tilt (10% by default)
    [Range(0, 100)]
    private int tiltProbability = 10;

    // Probability of obstacle having a vertical movement (5% by default)
    [Range(0, 100)]
    private int verticalMoveProbability = 5;

    // Scales limits for x size of generated obstacle
    private float minScaleX = 0.2f;
    private float maxScaleX = 1f;

    // Scales limits for y size of generated obstacle
    private float minScaleY = 0.5f;
    private float maxScaleY = 4f;

    // Minimum vertical speed for obstacles
    private float minVerticalSpeed = 0.5f;

    // Maximum vertical speed for obstacles
    private float maxVerticalSpeed = 1.0f;

    // Maximum spawn interval to reach (adjust as needed)
    private float maxSpawnIntervalToReach = 5.0f;

    // Time after which the game starts getting harder
    private float timeToStartHarder = 30.0f;

    // Delay before introducing moving and tilted obstacles
    private float delayBeforeMovingAndTilted = 15.0f;

    // Time interval for moving and tilted obstacles after the delay
    private float movingAndTiltedSpawnInterval = 2.0f;

    // Boolean flag to track if moving and tilted obstacles are introduced
    private bool movingAndTiltedIntroduced = false;

    private float timeSinceLastSpawn = 0.0f;

    /// Should we spawn obstacles?
    public bool spawnObstacles = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnObstacles) {
            timeSinceLastSpawn += Time.deltaTime;

            // Generate obstacle with random parameters
            if (timeSinceLastSpawn >= GetRandomSpawnInterval())
            {
                SpawnObstacle();
                timeSinceLastSpawn = 0.0f;
            }

            // Increase obstacle speed over time
            obstacleSpeed += speedIncreaseRate * Time.deltaTime;

            // Check if it's time to start introducing moving and tilted obstacles
            if (Time.timeSinceLevelLoad >= delayBeforeMovingAndTilted && !movingAndTiltedIntroduced)
            {
                movingAndTiltedIntroduced = true;
                minSpawnInterval = movingAndTiltedSpawnInterval;
                maxSpawnInterval = movingAndTiltedSpawnInterval;
            }

            // Decrease spawn interval over time until it reaches the minimum and increase verticalMoveProbability
            if (Time.timeSinceLevelLoad >= timeToStartHarder)
            {
                minSpawnInterval = Mathf.Max(minSpawnInterval - Time.deltaTime / 60.0f, 0.5f);
                maxSpawnInterval = Mathf.Max(maxSpawnInterval - Time.deltaTime / 60.0f, 1.5f);

                // Encrease timeToStartHarder every timeToStartHarder by 5
                verticalMoveProbability = Math.Min(verticalMoveProbability + (int)Time.deltaTime / (int)timeToStartHarder * 5, 100);
            }
        } else {
            obstacleSpeed = 0.0f;
        }
    }

    float GetRandomSpawnInterval()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnObstacle()
    {
        // Random coordinates for obstacle spawn
        float randomY = Random.Range(-4.0f, 4.0f);

        // Random scale for the obstacle
        float randomScaleX = Random.Range(minScaleX, maxScaleX);
        float randomScaleY = Random.Range(minScaleY, maxScaleY);

        // Random rotation for the obstacle (tilt)
        Quaternion obstacleRotation = Quaternion.identity;
        if (Random.Range(0, 100) < tiltProbability)
        {
            float tiltAngle = Random.Range(20.0f, 70.0f) * (Random.value >= 0.5 ? 1.0f : -1.0f); // Multiplied by random 1 or -1
            obstacleRotation = Quaternion.Euler(0.0f, 0.0f, tiltAngle);
        }

        // Random vertical movement for the obstacle
        float verticalSpeed = 0.0f;
        if (Random.Range(0, 100) < verticalMoveProbability)
        {
            verticalSpeed = Random.Range(minVerticalSpeed, maxVerticalSpeed);
            // Determine direction (up or down)
            if (Random.Range(0, 2) == 0)
            {

                verticalSpeed = -verticalSpeed;
            }
        }

        // Create obstacle
        GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(10.0f, randomY, 0), obstacleRotation);
        obstacle.transform.localScale = new Vector3(randomScaleX, randomScaleY, 1.0f);

        // Assign movement script to the obstacle
        ObstacleMovement obstacleMovement = obstacle.GetComponent<ObstacleMovement>();
        if (obstacleMovement != null)
        {
            obstacleMovement.speed = obstacleSpeed;
            obstacleMovement.verticalSpeed = verticalSpeed;
        }
    }
}
