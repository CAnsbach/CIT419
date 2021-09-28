using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask _aimLayerMask;
    GameController gc;
    public Camera camera;
    public Transform firingPoint;
    public GameObject bullet;

    const float speed = 5.0f;
    int health = 100;

    CharacterController controller;

    Vector3 velocity;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();        
        controller = gameObject.AddComponent<CharacterController>();
    }

    private void Update()
    {

        if(health <= 0)
        {
            Dead();
        }

        else
        {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            controller.Move(moveDirection * Time.deltaTime * speed);

            velocity.y += -9.81f * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Instantiate(bullet, transform.position, transform.rotation);
            }
        }
    }

    private void LateUpdate()
    {
        if(!gc.paused)
        {
            LookAtMouse();
        }
    }

    void LookAtMouse()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _aimLayerMask));
        {
            var destination = hitInfo.point;
            destination.y = transform.position.y;

            Vector3 direction = destination - transform.position;
            direction.Normalize();
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage >= health)
        {
            health = 0;
            Dead();
        }
        else
        {
            health -= damage;
        }
    }

    void Dead()
    {
        gc.PlayerDeath();
    }
}
