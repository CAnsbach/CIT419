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
        //Get the button
        Button btn = GetComponent<Button>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();

        btn.onClick.AddListener(StartGame);
    }

    /// <summary>
    /// Method used to tell the GameController that the game has started.
    /// </summary>
    void StartGame()
    {
        gc.GameStart(); 
    }
}
