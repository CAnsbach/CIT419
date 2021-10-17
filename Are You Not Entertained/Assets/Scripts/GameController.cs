using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance 
    { 
        get 
        { 
            return _instance; 
        } 
    }

    private string username;

    public string getUsername()
    {
        return username;
    }

    public void setUsername(string username)
    {
        this.username = username;
    }

    private int score;

    public int GetScore()
    {
        return score;
    }

    public bool paused, updateScore;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(this);
    }

    int totalEnemies;
    float betweenWaves = 10f, nextWaveTime = 0;
    bool startNextWave = true, started = false;
    List<GameObject> enemies = new List<GameObject>();

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game" && !started)
        {
            NextWave();
            started = true;
        }
        if (startNextWave && Time.time >= nextWaveTime)
        {
            NextWave();
            startNextWave = false;
        }
    }

    public void PlayerDeath()
    {
        updateScore = true;
        SceneManager.LoadScene("DeathScreen");
        started = false;
    }

    public void UpdateScore(int score)
    {
        this.score += score;
    }

    public void GameStart()
    {
        score = 0;

        SceneManager.LoadScene("Game");
    }

    public void NextWave()
    {
        totalEnemies = 0;
        enemies.Clear();
        foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            spawner.GetComponent<Spawner>().SpawnEnemy();
        }
    }

    public void NextTrainingWave()
    {
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("Spawner");

        enemySpawners[Random.Range(0, enemySpawners.Length)].GetComponent<Spawner>().SpawnEnemy();
    }

    public void VolumeUpdated()
    {
        float volume = PlayerPrefs.GetFloat("VolumeLevel", .75f);
    }

    public void AddSelf(GameObject enemy)
    {
        enemies.Add(enemy);
        totalEnemies++;
        Debug.Log("Enemy total: " + totalEnemies);

    }

    public void RemoveSelf(GameObject enemy)
    {
        enemies.Remove(enemy);
        totalEnemies--;

        Debug.Log("The Score is now: " + score);

        if(totalEnemies == 0)
        {
            nextWaveTime = Time.time + betweenWaves;
            startNextWave = true;
        }
        else
        {
            Debug.Log("Enemies Remaining: " + totalEnemies);
        }
    }
}
