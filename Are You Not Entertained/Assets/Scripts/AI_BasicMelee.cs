using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_BasicMelee : AI_GeneralController
{
    public enum State { Chase, Attack };
    public State currentState;
    int damage;

    GameObject player;
    Transform playerT;
    NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerT = player.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Chase;
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
