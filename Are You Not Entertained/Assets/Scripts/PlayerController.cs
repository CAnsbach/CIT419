using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float xDirection, yDirection;
    const float speed = 5f;
    int health;
    Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        xDirection = Input.GetAxis("Horizontal");
        yDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, yDirection);

        rigidbody.AddForce(moveDirection * speed);
    }
}
