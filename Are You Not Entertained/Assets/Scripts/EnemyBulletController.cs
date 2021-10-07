using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    const int damage = 5;

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
