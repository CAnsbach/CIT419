using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    const int damage = 5;

    private void Start()
    {
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        transform.position = transform.position + transform.forward * 20f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
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

    void Gone()
    {
        Destroy(gameObject);
    }
}
