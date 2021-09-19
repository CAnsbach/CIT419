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

    void UpdateWelcome()
    {
        UsernameIntro.text = "Welcome, " + gc.getUsername() + "!";
    }
}
