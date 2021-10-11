using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_BasicMelee : AI_GeneralController
{
    public enum State { Chase, Attack };
    public State currentState;
    int damage = 10;

    public GameObject player;
    Transform playerT;
    NavMeshAgent agent;

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        playerT = player.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        ChangeStates(State.Chase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MLBoss"))
        {
            ChangeStates(State.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MLBoss"))
        {
            ChangeStates(State.Chase);
        }
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
            if (player.CompareTag("Player"))
            {
                player.GetComponent<PlayerController>().TakeDamage(damage);
                
            }

            else
            {
                Debug.Log("Attacking " + player.tag);
                player.GetComponent<AI_MLABoss>().Hit(damage);
            }

            yield return new WaitForSeconds(.5f);

        }
        yield return null;
    }
}
