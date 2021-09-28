using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MLBoss : MonoBehaviour
{
    GameController gc;
    int health = 150;
    int score = 100;

    private void Start()
    {
        gc = gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void Hit(int damage)
    {
        if (damage >= health)
        {
            Die();
        }

        else
        {
            health -= damage;
        }
    }

    void Die()
    {
        Debug.Log("Dieing");
        gc.UpdateScore(score);
        Debug.Log("Score is now: " + gc.GetScore());
        Destroy(gameObject);
    }
}
