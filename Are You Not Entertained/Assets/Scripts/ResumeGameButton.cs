using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeGameButton : MonoBehaviour
{
    GameObject pauseMenu;

    void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").gameObject;

        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(ResumeGame);
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
