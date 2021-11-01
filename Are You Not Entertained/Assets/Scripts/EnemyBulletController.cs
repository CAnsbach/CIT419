using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBulletController : MonoBehaviour
{
    const int damage = 5;
    bool training;

    private void Start()
    {
        //Destroy this game object after 15 seconds
        Destroy(gameObject, 15f);
        //Determine if this is the training scene or not
        training = SceneManager.GetActiveScene().name == "MLTraining";
    }

    void FixedUpdate()
    {
        //Update the bullet's position
        transform.position += transform.forward * 20f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Determine what it hit and if it is training to act accordingly
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Gone();
        }
        else if (collision.gameObject.tag.Equals("MLBoss") && training)
        {
            collision.gameObject.GetComponent<AI_MLABoss>().Hit(damage);
            Gone();
        }
        else
        {
            Gone();
        }
    }

    /// <summary>
    /// Method used to destroy the game object when it collides with something
    /// </summary>
    void Gone()
    {
        Destroy(gameObject);
    }
}
