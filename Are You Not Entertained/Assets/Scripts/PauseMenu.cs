using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    GameObject pauseMenu;
    GameController gc;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").gameObject;

        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeSelf)
            {
                OpenPauseMenu();
            }

            else
            {
                ClosePauseMenu();
            }
        }
    }

    void OpenPauseMenu()
    {
        Time.timeScale = 0;
        gc.paused = true;
        pauseMenu.SetActive(true);
    }

    void ClosePauseMenu()
    {
        Time.timeScale = 1;
        gc.paused = false;
        pauseMenu.SetActive(false);
    }
}
