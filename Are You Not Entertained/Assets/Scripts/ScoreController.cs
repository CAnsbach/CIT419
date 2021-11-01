using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    GameController gc;
    TMP_Text scoreText;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        scoreText = this.GetComponent<TMP_Text>();
        
        gc.scoreUpdate.AddListener(UpdateScore);
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + gc.GetScore().ToString();
    }
}
