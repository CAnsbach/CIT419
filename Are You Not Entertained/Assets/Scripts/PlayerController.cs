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
    
    const float speed = 5.0f, blockCooldown = 10.0f, blockDuration = 6.0f, meleeRange = 2.0f;
    public bool canBlock = true, blocking = false;
    int health = 100;
    const int meleeDamage = 30;
    float stopWatch, blockStop, damageAgain;

    CharacterController controller;

    Vector3 velocity;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();        
        controller = gameObject.AddComponent<CharacterController>();
    }

    private void Update()
    {
        if (!canBlock && Time.time > stopWatch)
        {
            canBlock = true;
            Debug.Log("Can Block Again");
        }

        if(blocking && Time.time > blockStop)
        {
            blocking = false;
            Debug.Log("No longer blocking");
        }

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
                FireGun();
            }

            else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                SwordStrike();
            }

            if(Input.GetKeyDown(KeyCode.LeftShift) && canBlock)
            {
                BlockAbility();
            }

            if(Input.GetKeyDown(KeyCode.F))
            {
                Dead();
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

    void BlockAbility()
    {
        Debug.Log("Blocking");
        canBlock = false;
        blocking = true;

        stopWatch = Time.time + blockCooldown;
        blockStop = Time.time + blockDuration;
    }

    void FireGun()
    {
        GameObject bulletGO = Instantiate(bullet, firingPoint.position, Quaternion.identity);

        Rigidbody bulletRB = bulletGO.GetComponent<Rigidbody>();

        bulletRB.AddForce(firingPoint.forward * 20f, ForceMode.Impulse);
    }

    void SwordStrike()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, meleeRange))
        {
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log("Hit: " + hitObject.tag);

            if (hitObject.tag.Equals("EnemyMelee"))
            {
                hitObject.GetComponent<AI_BasicMelee>().Hit(meleeDamage);
            }

            else if (hitObject.tag.Equals("EnemyRanged"))
            {
                hitObject.GetComponent<AI_BasicRanged>().Hit(meleeDamage);
            }

            else if (hitObject.tag.Equals("MLBoss"))
            {
                hitObject.GetComponent<AI_MLBoss>().Hit(meleeDamage);
            }
            else
            {
                Debug.Log("I hit: " + hitObject.tag);
            }
        }
        else
        {
            Debug.Log("None");
        }
        
    }

    public void TakeDamage(int damage)
    {
        if (!blocking)
        {
            if (damage >= health)
            {
                health = 0;
                Dead();
            }
            else
            {
                health -= damage;
                Debug.Log(health);
            }
        }

        else
        {
            Debug.Log("Blocked Damage");
        }
    }

    void Dead()
    {
        gc.PlayerDeath();
    }
}
