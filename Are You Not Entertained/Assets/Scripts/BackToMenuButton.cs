using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenuButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
