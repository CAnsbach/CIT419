using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetUsername : MonoBehaviour
{
    public Text UsernameIntro;
    GameController gc;

    private void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        UpdateWelcome();
    }

    /// <summary>
    /// Method used to update the message shown in the main menu
    /// </summary>
    void UpdateWelcome()
    {
        UsernameIntro.text = "Welcome, " + gc.getUsername() + "!";
    }
}
