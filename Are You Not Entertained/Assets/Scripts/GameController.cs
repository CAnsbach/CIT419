using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public UnityEvent scoreUpdate;

    public static GameController Instance 
    { 
        get 
        { 
            return _instance; 
        } 
    }

    //Username of the user with getter and setter
    private string username;

    public string getUsername()
    {
        return username;
    }

    public void setUsername(string username)
    {
        this.username = username;
    }

    //Score of the user with a getter
    private int score;

    public int GetScore()
    {
        return score;
    }

    public bool paused, updateScore;

    private void Awake()
    {
        //Singleton
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

    int totalEnemies;                                               //Total enemies in wave
    float betweenWaves = 10f, nextWaveTime = 0;                     //Time between waves and time to start next wave
    bool startNextWave = true, started = false, healPlayer = false; //Booleans for determining if a new wave is to start, the player should be healed, or if the game has started
    List<GameObject> enemies = new List<GameObject>();              //List of enemies (enemies remaining in current wave)
    public TMP_Text scoreTxt;

    private void Start()
    {
        if (scoreUpdate == null)
        {
            scoreUpdate = new UnityEvent();
        }
    }

    private void Update()
    {
        //If the game is the active scene and it has not started, start it
        if (SceneManager.GetActiveScene().name == "Game" && !started)
        {
            NextWave();
            started = true;
        }
        //Else if it is time to start the next wave, send it
        else if (startNextWave && Time.time >= nextWaveTime)
        {
            NextWave();
            startNextWave = false;
        }
    }

    /// <summary>
    /// Setter and getter for the healPlayer boolean
    /// </summary>
    public void PlayerHealed()
    {
        healPlayer = false;
    }

    public bool ShouldHeal()
    {
        return healPlayer;
    }

    /// <summary>
    /// Method used to handle the player's death
    /// </summary>
    public void PlayerDeath()
    {
        updateScore = true;                     //Update the score
        SceneManager.LoadScene("DeathScreen");  //Load the death scene
        started = false;                        //The game has ended
    }

    /// <summary>
    /// Method used to update the user's score
    /// </summary>
    /// <param name="score">Value to add to the user's score</param>
    public void UpdateScore(int score)
    {
        this.score += score;
        scoreUpdate.Invoke();
    }

    /// <summary>
    /// Method used to handle starting the game
    /// </summary>
    public void GameStart()
    {
        score = 0;                      //Reset score

        SceneManager.LoadScene("Game"); //Load the Game scene
    }

    /// <summary>
    /// Method used to spawn the next wave
    /// </summary>
    public void NextWave()
    {
        totalEnemies = 0;       //There are currently no enemies
        enemies.Clear();        //Clear the list of all GameObjects
        //For each spawner in the Game scene, spawn their assigned enemy.
        foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            spawner.GetComponent<Spawner>().SpawnEnemy();
        }
    }

    /// <summary>
    /// Method used to spawn enemies for training
    /// </summary>
    public void NextTrainingWave()
    {
        //Get all spawners in the scene
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("Spawner");

        //Tell a random spawner to spawn their assigned enemy
        enemySpawners[Random.Range(0, enemySpawners.Length)].GetComponent<Spawner>().SpawnEnemy();
    }

    /// <summary>
    /// Method used to get the preferred volume of the player
    /// </summary>
    public void VolumeUpdated()
    {
        float volume = PlayerPrefs.GetFloat("VolumeLevel", .75f);
    }

    /// <summary>
    /// Method used to add an enemy to the enemy list
    /// </summary>
    /// <param name="enemy">Enemy GameObject to add to the list</param>
    public void AddSelf(GameObject enemy)
    {
        enemies.Add(enemy);                         //Add the enemy to the list
        totalEnemies++;                             //Increment the number of enemies
    }

    /// <summary>
    /// Method used to remove an enemy from the enemy list
    /// </summary>
    /// <param name="enemy">Enemy Game Object to add to the list</param>
    public void RemoveSelf(GameObject enemy)
    {
        enemies.Remove(enemy);                      //Remove the enemy
        totalEnemies--;                             //Decrement the number of enemies

        //If there are no remaining enemies, prepare for the next wave
        if(totalEnemies == 0)
        {
            nextWaveTime = Time.time + betweenWaves;
            startNextWave = true;
            healPlayer = true;
        }
    }
}
