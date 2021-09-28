using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    GameController gc;
    void Start()
    {
        Button btn = GetComponent<Button>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();

        btn.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        gc.GameStart(); 
    }
}
