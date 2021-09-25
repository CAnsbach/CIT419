using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeGameButton : MonoBehaviour
{
    GameController gc;
    GameObject pauseMenu;

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").gameObject;

        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(ResumeGame);
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        gc.paused = false;
        pauseMenu.SetActive(false);
    }
}
