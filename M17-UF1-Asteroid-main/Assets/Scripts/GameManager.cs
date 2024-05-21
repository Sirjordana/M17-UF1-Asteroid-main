using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; // Array of asteroid prefabs
    public float spawnRadius = 10f;
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    public TextMeshProUGUI waveText;
    private int currentWave = 1;
    private int asteroidsPerWave = 5;
    private float spawnInterval = 1f;
    private float minSpawnInterval = 0.2f;
    private float spawnIntervalDecrement = 0.05f;

    public bool InGame = false;

    public TextMeshProUGUI HiScoreTxt;
    public int HiScore;
    public TextMeshProUGUI ScoreTxt;
    public int Score;
    public TextMeshProUGUI NewHiScore;

    public GameObject restartBtn;
    public GameObject startBtn;

    public GameObject player;

    public int health;
    public Image[] lives;

    void Start()
    {
        startBtn.SetActive(true);
        restartBtn.SetActive(false);
        NewHiScore.enabled = false;
        HiScore = PlayerPrefs.GetInt("HiScore");
        updateHiScore();
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            waveText.text = "Wave: " + currentWave;

            for (int i = 0; i < asteroidsPerWave; i++)
            {
                SpawnAsteroid();
                yield return new WaitForSeconds(spawnInterval);
            }

            // Increase wave difficulty
            asteroidsPerWave++;
            currentWave++;

            // Adjust spawn interval
            if (spawnInterval > minSpawnInterval)
                spawnInterval -= spawnIntervalDecrement;

            yield return new WaitForSeconds(5f); // Wait before starting next wave
        }
    }

    void SpawnAsteroid()
    {
        // Get center of the screen
        Vector3 centerScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));

        // Generate random position within the circle
        Vector2 randomPos = Random.insideUnitCircle.normalized * spawnRadius;

        // Calculate direction towards the center of the screen with a slight variation
        Vector2 directionToCenter = (centerScreen - (Vector3)randomPos).normalized;
        float randomAngle = Random.Range(-10f, 10f);
        Quaternion randomRotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        Vector2 finalDirection = randomRotation * directionToCenter;

        // Randomly select asteroid prefab
        int randomIndex = Random.Range(0, Mathf.RoundToInt(Mathf.Min(currentWave, asteroidPrefabs.Length))); // Ensure we don't exceed prefab array length
        GameObject asteroidPrefab = asteroidPrefabs[randomIndex];

        // Create asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, randomPos, Quaternion.identity);

        // Apply random speed
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        rb.velocity = finalDirection * Random.Range(minSpeed, maxSpeed);
    }

    public void addPoints(int baseScore)
    {
        Score += (baseScore * 5);
        ScoreTxt.text = "Score: " + Score;
    }

    public void updateHiScore()
    {
        if (Score > HiScore)
        {
            HiScore = Score;
            NewHiScore.enabled = true;
        }
        HiScoreTxt.text = "High Score: " + HiScore;
        PlayerPrefs.SetInt("HiScore", HiScore);
    }

    public void TakeDamage()
    {
        health--;
        lives[health].enabled = false;
        
        if (health == 0)
        {
            InGame = false;
            StopAllCoroutines();
            foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("Asteroid"))
            {
                Destroy(asteroid);
            }
            restartBtn.SetActive(true);
            updateHiScore();
        }
    }

    public void Restart()
    {
        restartBtn.SetActive(false);
        health = 3;
        foreach ( Image live in lives )
        {
            live.enabled = true;
        }
        NewHiScore.enabled = false;
        Score = 0;
        ScoreTxt.text = "Score: " + Score;
        currentWave = 0;
        waveText.text = "Wave: " + currentWave;
        currentWave = 1;
        asteroidsPerWave = 5;
        spawnInterval = 1f;
        StartCoroutine(SpawnWaves());
        InGame = true;
    }

    public void StartGame()
    {
        health = 3;
        startBtn.SetActive(false);
        StartCoroutine(SpawnWaves());
        InGame = true;
    }
}
