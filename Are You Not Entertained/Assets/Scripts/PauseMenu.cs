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
        gameUI.GetComponentInChildren<TMP_Text>().alpha = .2f;
        pauseMenu.SetActive(true);
    }

    void ClosePauseMenu()
    {
        Time.timeScale = 1;
        gc.paused = false;
        gameUI.GetComponentInChildren<TMP_Text>().alpha = 1f;
        pauseMenu.SetActive(false);
    }
}
