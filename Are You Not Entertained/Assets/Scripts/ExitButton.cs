using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    GameController gc;

    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Quit);
    }

    void Quit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}

