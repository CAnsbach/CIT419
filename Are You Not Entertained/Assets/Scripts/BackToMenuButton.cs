using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenuButton : MonoBehaviour
{
    GameController gc;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(GoToMainMenu);
    }

    /// <summary>
    /// Method used to load the main menu scene
    /// </summary>
    void GoToMainMenu()
    {
        Time.timeScale = 1;                 //Restart time
        gc.paused = false;                  //Game is no longer paused
        SceneManager.LoadScene("MainMenu"); //Load the MainMenu Scene
    }
}
