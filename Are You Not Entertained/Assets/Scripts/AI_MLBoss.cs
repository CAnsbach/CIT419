using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_MLBoss : MonoBehaviour
{
    public enum State { Chase, Attack };
    public State currentState;
    int damage = 10;

    GameObject player;
    Transform playerT;
    NavMeshAgent agent;
    GameController gc;
    int health = 150;
    int score = 100;

    private void Start()
    {
        gc = gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerT = player.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        ChangeStates(State.Chase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ChangeStates(State.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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

    void Die()
    {
        Debug.Log("Dieing");
        gc.UpdateScore(score);
        Debug.Log("Score is now: " + gc.GetScore());
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
            player.GetComponent<PlayerController>().TakeDamage(damage);
            yield return new WaitForSeconds(.5f);
        }
        yield return null;
    }
}
