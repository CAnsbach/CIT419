using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResumeGameButton : MonoBehaviour
{
    GameController gc;
    GameObject pauseMenu, gameUI;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").gameObject;
        gameUI = GameObject.FindGameObjectWithTag("GameUI").gameObject;

        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(ResumeGame);
    }

    /// <summary>
    /// Method used to resume the game
    /// </summary>
    void ResumeGame()
    {
        Time.timeScale = 1;                                     //Start time again
        gc.paused = false;                                      //Game is no longer paused
        //Set the player UI's alpha back to 1
        foreach (TMP_Text text in gameUI.GetComponentsInChildren<TMP_Text>())
        {
            text.alpha = 1f;
        }
        pauseMenu.SetActive(false);                             //Deactivate the pause menu
    }
}
