using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AI_BasicRanged : MonoBehaviour
{
    public enum State { Chase, Shoot};          //States for the state machine
    public State currentState;
    public GameObject bullet;
    public Transform shooter;

    bool training;

    GameObject player;
    Transform playerT;
    NavMeshAgent agent;

    const float range = 20f;
    GameController gc;
    int health = 50, score = 10;

    private void Awake()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        gc.AddSelf(gameObject);                                         //Add self to the enemy list

        training = SceneManager.GetActiveScene().name == "MLTraining";  //Determine if training

        //Set the target as necessary based on if the ML AI is training or not
        if (training)
        {
            player = GameObject.FindGameObjectWithTag("MLBoss");
        }

        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        playerT = player.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        ChangeStates(State.Chase);                                      //Default state is chase
    }

    private void FixedUpdate()
    {
        //If the player get's in firing range, change from chase to shoot
        float distance = Vector3.Distance(playerT.position, transform.position);

        if (distance <= range && currentState != State.Shoot)
        {
            ChangeStates(State.Shoot);
        }
        else if (distance > range && currentState != State.Chase)
        {
            ChangeStates(State.Chase);
        }
    }

    /// <summary>
    /// Method used to have the AI take damage
    /// </summary>
    /// <param name="damage">Amount of damage to take</param>
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
        StopAllCoroutines();
        gc.UpdateScore(score);              //Update the player's score
        gc.RemoveSelf(gameObject);          //Remove self from enemy list
        Destroy(gameObject);                //Destroy self
    }

    /// <summary>
    /// State machine
    /// </summary>
    /// <param name="current">Current state</param>
    private void ChangeStates(State current)
    {
        if (current == State.Chase)
        {
            currentState = State.Chase;
            StopAllCoroutines();
            StartCoroutine("AI_Chase");
        }
        else if (current == State.Shoot)
        {
            currentState = State.Shoot;
            StopAllCoroutines();
            StartCoroutine("AI_Shoot");
        }
    }

    /// <summary>
    /// Coroutine used to chase the target
    /// </summary>
    /// <returns></returns>
    IEnumerator AI_Chase()
    {
        agent.isStopped = false;                                //Agent is no longer stopped
        while (currentState == State.Chase && player != null)
        {
            try
            {
                agent.SetDestination(playerT.position);
                transform.LookAt(player.transform.position);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            yield return new WaitForSeconds(.2f);
        }
        yield return null;
    }

    IEnumerator AI_Shoot()
    {
        agent.isStopped = true;                                 //Stop the agent to shoot at the player
        while (player != null)
        {
            //Look at the player, instantiate a bullet, and set the bullet's direection
            transform.LookAt(player.transform.position);

            GameObject bulletGO = Instantiate(bullet, shooter.position, Quaternion.identity);

            bulletGO.transform.forward = shooter.forward;

            yield return new WaitForSeconds(.4f);

            transform.LookAt(player.transform.position);
        }

        yield return null;
    }
}
