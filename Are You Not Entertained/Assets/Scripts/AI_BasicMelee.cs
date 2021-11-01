using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AI_BasicMelee : MonoBehaviour
{
    public enum State { Chase, Attack };        //States for the state machine
    public State currentState;
    int damage = 10;

    public GameObject player;
    Transform playerT;
    NavMeshAgent agent;

    GameController gc;
    int health = 50, score = 10;

    const float attackCooldown = 1f;
    float attackTracker;
    bool canAttack = true;
    bool training;

    private void Awake()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        gc.AddSelf(gameObject);         //Add self to the list of enemies

        //Determine if the ML AI is training
        training = SceneManager.GetActiveScene().name == "MLTraining";

        //If it is training, the "player" is the ML AI
        if (training)
        {
            player = GameObject.FindGameObjectWithTag("MLBoss");
        }

        //Else, target the player
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        playerT = player.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        ChangeStates(State.Chase);                  //Start in the chase state
    }

    private void Update()
    {
        //If the AI can't attack and the cooldown is done, the AI can attack again
        if (!canAttack && Time.time > attackTracker)
        {
            canAttack = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Determine what entered the trigger and act accordingly
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("MLBoss") && training))
        {
            ChangeStates(State.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Determine what left the trigger and act accordingly
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("MLBoss") && training))
        {
            ChangeStates(State.Chase);
        }
    }

    /// <summary>
    /// Method used to have the AI take damage
    /// </summary>
    /// <param name="damage">Damage for the AI to take</param>
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

    /// <summary>
    /// Method used to kill the AI
    /// </summary>
    public void Die()
    {
        StopAllCoroutines();                //Stop chasing or attacking
        gc.UpdateScore(score);              //Update the player's score
        gc.RemoveSelf(gameObject);          //Remove self from the enemy list
        Destroy(gameObject);                //Destroy the GameObject
    }

    /// <summary>
    /// State machine
    /// </summary>
    /// <param name="current">State to change to</param>
    private void ChangeStates(State current)
    {
        if (current == State.Attack)
        {
            currentState = State.Attack;
            StopAllCoroutines();
            StartCoroutine("AI_Attack");
        }
        else if (current == State.Chase)
        {
            currentState = State.Chase;
            StopAllCoroutines();
            StartCoroutine("AI_Chase");
        }
    }

    /// <summary>
    /// Coroutine to chase the target
    /// </summary>
    /// <returns></returns>
    IEnumerator AI_Chase()
    {
        while (currentState == State.Chase && player != null)
        {
            try
            {
                agent.SetDestination(playerT.position);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            yield return new WaitForSeconds(.2f);
        }
        yield return null;
    }

    /// <summary>
    /// Coroutine to attack the target
    /// </summary>
    /// <returns></returns>
    IEnumerator AI_Attack()
    {
        agent.SetDestination(gameObject.transform.position);

        while (currentState == State.Attack && player != null)
        {
            //If the AI can attack, do so
            if (canAttack)
            {
                if (player.CompareTag("Player"))
                {
                    player.GetComponent<PlayerController>().TakeDamage(damage);
                    canAttack = false;

                    attackTracker = Time.time + attackCooldown;     //Set the cooldown
                }

                else if (training)
                {
                    player.GetComponent<AI_MLABoss>().Hit(damage);
                    canAttack = false;

                    attackTracker = Time.time + attackCooldown;     //Set the cooldown
                }
            }

            yield return new WaitForSeconds(.5f);

        }
        yield return null;
    }
}
