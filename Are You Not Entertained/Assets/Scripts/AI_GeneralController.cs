using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_GeneralController : MonoBehaviour
{
    public GameController gc;
    int health = 50;
    int score = 10;

    private void Awake()
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

    public void Die()
    {
        Debug.Log("Dieing");
        gc.UpdateScore(score);
        Debug.Log("Score is now: " + gc.GetScore());
        Destroy(gameObject);
    }
}
