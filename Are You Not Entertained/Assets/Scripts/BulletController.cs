using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    const int damage = 10;

    void Update()
    {
        transform.position = transform.position + transform.forward * 20f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
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
            collision.gameObject.GetComponent<AI_MLBoss>().Hit(damage);
            Gone();
        }
        else
        {
            Debug.Log("I hit: " + collision.gameObject.tag);
            Gone();
        }
    }

    void Gone()
    {
        Destroy(gameObject);
    }
}
