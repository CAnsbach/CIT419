using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    const int damage = 10;

    private void Start()
    {
        //Destroy this game object after 15 seconds
        Destroy(gameObject, 15f);
    }

    void FixedUpdate()
    {
        //Update the position of the bullet
        transform.position += transform.forward * 20f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Determine what the bullet hit and act accordingly
        if (collision.gameObject.tag.Equals("EnemyMelee"))
        {
            collision.gameObject.GetComponent<AI_BasicMelee>().Hit(damage);
            Gone();
        }

        else if (collision.gameObject.tag.Equals("EnemyRanged"))
        {
            collision.gameObject.GetComponent<AI_BasicRanged>().Hit(damage);
            Gone();
        }

        else if (collision.gameObject.tag.Equals("MLBoss"))
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
