using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_BasicRanged : AI_GeneralController
{
    public enum State { Chase, Shoot};
    public State currentState;
    public GameObject bullet;
    public Transform shooter;

    GameObject player;
    Transform playerT;
    RaycastHit check;
    NavMeshAgent agent;
    int damage = 5;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerT = player.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        ChangeStates(State.Chase);
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(playerT.position, transform.position);

        if (distance <= 10f && currentState != State.Shoot)
        {
            ChangeStates(State.Shoot);
        }
        else if (distance > 10f && currentState != State.Chase)
        {
            ChangeStates(State.Chase);
        }
    }


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

    IEnumerator AI_Chase()
    {
        agent.isStopped = false;
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
        agent.isStopped = true;
        while (player != null)
        {
            transform.LookAt(player.transform.position);

            GameObject bulletGO = Instantiate(bullet, shooter.position, Quaternion.identity);

            bulletGO.transform.forward = shooter.forward;

            yield return new WaitForSeconds(.2f);

            transform.LookAt(player.transform.position);
        }

        yield return null;
    }
}
