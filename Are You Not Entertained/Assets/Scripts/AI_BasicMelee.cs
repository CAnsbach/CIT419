using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AI_BasicMelee : MonoBehaviour
{
    public enum State { Chase, Attack };
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
        gc.AddSelf(gameObject);

        training = SceneManager.GetActiveScene().name == "MLTraining";

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
        ChangeStates(State.Chase);
    }

    private void Update()
    {
        if (!canAttack && Time.time > attackTracker)
        {
            canAttack = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("MLBoss") && training))
        {
            ChangeStates(State.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || (other.gameObject.CompareTag("MLBoss") && training))
        {
            ChangeStates(State.Chase);
        }
    }

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

    public void Die()
    {
        StopAllCoroutines();
        gc.UpdateScore(score);
        gc.RemoveSelf(gameObject);
        Destroy(gameObject);
    }

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

    IEnumerator AI_Attack()
    {
        agent.SetDestination(gameObject.transform.position);

        while (currentState == State.Attack && player != null)
        {

            if (canAttack)
            {
                if (player.CompareTag("Player"))
                {
                    player.GetComponent<PlayerController>().TakeDamage(damage);
                    canAttack = false;

                    attackTracker = Time.time + attackCooldown;
                }

                else if (training)
                {
                    player.GetComponent<AI_MLABoss>().Hit(damage);
                    canAttack = false;

                    attackTracker = Time.time + attackCooldown;
                }
            }

            yield return new WaitForSeconds(.5f);

        }
        yield return null;
    }
}
