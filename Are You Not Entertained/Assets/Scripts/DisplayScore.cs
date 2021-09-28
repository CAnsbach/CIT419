using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayScore : MonoBehaviour
{
    GameController gc;
    TMP_Text scoreText;
    
    void Start()
    {
        scoreText = this.GetComponent<TMP_Text>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        scoreText.SetText("Score: " + gc.GetScore());
    }
}
