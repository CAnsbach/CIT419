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

    private string password;

    public string getPassword()
    {
        return password;
    }

    public void setPassword(string password)
    {
        this.password = password;
    }

    private int score;

    public bool paused;

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
        SceneManager.LoadScene("DeathScreen");
    }

    public void UpdateScore(int score)
    {
        this.score += score;
    }

    public int GetScore()
    {
        return score;
    }

    public void GameStart()
    {
        score = 0;

        SceneManager.LoadScene("Game");
    }
}
