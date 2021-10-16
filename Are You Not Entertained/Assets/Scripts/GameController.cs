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

    private string username = "iliketurtles12343";

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



    public void PlayerDeath()
    {
        updateScore = true;
        SceneManager.LoadScene("DeathScreen");
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
}
