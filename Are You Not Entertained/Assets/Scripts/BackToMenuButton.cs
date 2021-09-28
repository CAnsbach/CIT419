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

    void GoToMainMenu()
    {
        Time.timeScale = 1;
        gc.paused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
