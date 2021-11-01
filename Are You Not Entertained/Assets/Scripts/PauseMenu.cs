using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    GameObject pauseMenu, gameUI;
    GameController gc;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").gameObject;
        gameUI = GameObject.FindGameObjectWithTag("GameUI").gameObject;

        pauseMenu.SetActive(false);
    }

    void Update()
    {
        //If the user presses Escape, pause the game if there is no current pause menu.
        //If there is already a pause menu, unpause the game.
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeSelf)
            {
                //Open the pause menu
                OpenPauseMenu();
            }

            else
            {
                //Close the pause menu
                ClosePauseMenu();
            }
        }
    }

    /// <summary>
    /// Method used to pause the game
    /// </summary>
    void OpenPauseMenu()
    {
        Time.timeScale = 0;                                     //Stop time
        gc.paused = true;                                       //Game is paused
        //Set the player UI's alpha to .2
        foreach (TMP_Text text in gameUI.GetComponentsInChildren<TMP_Text>())
        {
            text.alpha = .2f;
        }
        pauseMenu.SetActive(true);                              //Activate the pause menu
    }

    /// <summary>
    /// Method used to unpause the game
    /// </summary>
    void ClosePauseMenu()
    {
        Time.timeScale = 1;                                     //Restart time
        gc.paused = false;                                      //Game is no longer paused
        //Set the player UI's alpha back to 1
        foreach (TMP_Text text in gameUI.GetComponentsInChildren<TMP_Text>())
        {
            text.alpha = 1f;
        }
        pauseMenu.SetActive(false);                             //Deactivate the pause menu
    }
}
