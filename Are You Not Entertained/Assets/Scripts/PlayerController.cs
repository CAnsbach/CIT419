using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask _aimLayerMask;           //Layer mask used for aiming
    GameController gc;
    public Camera camera;
    public Transform firingPoint;
    public GameObject bullet;
    public TMP_Text healthTxt, blockTxt, readyTxt;

    const float speed = 5.0f, blockCooldown = 10.0f, blockDuration = 6.0f, meleeRange = 2.0f;
    bool canBlock = true, blocking = false;
    int health = 100;
    const int meleeDamage = 30;
    float stopWatch, blockStop;

    CharacterController controller;     //Character controller for movement

    Vector3 velocity;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();        
        controller = gameObject.AddComponent<CharacterController>();
        healthTxt.text = "Health: " + health;
    }

    private void Update()
    {
        //If the player cannot block and the cooldown in done, the player can block again
        if (!canBlock && Time.time > stopWatch)
        {
            canBlock = true;
            readyTxt.text = "Block: Available";
        }

        //If the player is blocking and the duration for the ability is over, the player is no longer blocking
        if(blocking && Time.time > blockStop)
        {
            blocking = false;
            blockTxt.text = "Not Blocking";
        }

        //If the player should be healed due to passing a wave, heal them and notify the gamecontroller.
        if (gc.ShouldHeal())
        {
            Heal(10);
            gc.PlayerHealed();
        }

        //If the player's health is less than or equal to zero, kill them
        if(health <= 0)
        {
            Dead();
        }

        //Else, check for input from the player
        else if (!gc.paused)
        {
            //Get the movement input of the user
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            //Move the player
            controller.Move(moveDirection * Time.deltaTime * speed);

            //If the player is in the air, have them drop realistically
            velocity.y += -9.81f * Time.deltaTime;

            //Move the player down
            controller.Move(velocity * Time.deltaTime);

            //If they press the left mouse button, fire
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                FireGun();
            }

            //Else if the press the right mouse button, strike
            else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                SwordStrike();
            }

            //If the player presses left shift, see if they can block
            if(Input.GetKeyDown(KeyCode.LeftShift) && canBlock)
            {
                BlockAbility();
            }
        }
    }

    private void LateUpdate()
    {
        //If the game is not paused, have the player look at the mouse cursor
        if(!gc.paused)
        {
            LookAtMouse();
        }
    }

    /// <summary>
    /// Method used to have the player look at the mouse cursor's position
    /// </summary>
    void LookAtMouse()
    {
        //Get the position of the mouse
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        //If the raycast hits the aim layer, look at it.
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _aimLayerMask));
        {
            //Destination to look at
            var destination = hitInfo.point;
            destination.y = transform.position.y;

            //Direction to look in
            Vector3 direction = destination - transform.position;
            direction.Normalize();

            //Look in the direction of the mouse
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    /// <summary>
    /// Method used to perform the block ability
    /// </summary>
    void BlockAbility()
    {
        canBlock = false;
        blocking = true;

        stopWatch = Time.time + blockCooldown;
        blockStop = Time.time + blockDuration;

        readyTxt.text = "Block: On Cooldown";
        blockTxt.text = "Blocking";
    }

    /// <summary>
    /// Method used to fire a bullet
    /// </summary>
    void FireGun()
    {
        //Instantiate the bullet and set the bullet's direction
        GameObject bulletGO = Instantiate(bullet, firingPoint.position, Quaternion.identity);

        bulletGO.transform.forward = firingPoint.forward;
    }

    /// <summary>
    /// Method used to perform a melee strike
    /// </summary>
    void SwordStrike()
    {
        RaycastHit hit;

        //Send out a Raycast and if it hits something, determine what it hit and act accordingly
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, meleeRange))
        {
            GameObject hitObject = hit.transform.gameObject;

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
                hitObject.GetComponent<AI_MLABoss>().Hit(meleeDamage);
            }
        }
    }

    /// <summary>
    /// Method used to heal the player
    /// </summary>
    /// <param name="health">Amount to heal the player</param>
    public void Heal(int health)
    {
        //If the heal will put the player at or above 100, set their health to 100
        if (this.health + health >= 100)
        {
            this.health = 100;                          //Set the health to 100
            healthTxt.text = "Health: " + this.health;       //Update the UI
        }
        else
        {
            this.health += health;                      //Heal the player
            healthTxt.text = "Health: " + this.health;       //Update the UI
        }
    }

    /// <summary>
    /// Method used to have the player take damage
    /// </summary>
    /// <param name="damage">Amount of damage that is to be dealt to the player</param>
    public void TakeDamage(int damage)
    {
        //If the plyaer is not blocking, they take damage
        if (!blocking)
        {
            //If the damage is greater than or equal to the player's current health, kill them
            if (damage >= health)
            {
                health = 0;                                 //Set health to zero
                Dead();                                     //Kill the player
            }

            //Else, update their health
            else
            {
                health -= damage;                           //Update health
                healthTxt.text = "Health: " + health;       //Update the UI
            }
        }

        //Else, they do not take damage
        else
        {
            Debug.Log("Blocked Damage");
        }
    }

    /// <summary>
    /// Method used to kill the player
    /// </summary>
    void Dead()
    {
        gc.PlayerDeath();       //Notify the GameController of the player's death
    }
}
