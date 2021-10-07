using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_BasicRanged : AI_GeneralController
{
    public enum State { Chase, Attack, Shoot};
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
        if (Physics.Raycast(shooter.position, shooter.forward, out check, 8f))
        {
            if (check.collider.gameObject.CompareTag("Player") && currentState != State.Shoot)
            {
                ChangeStates(State.Shoot);
            }
        }

        Debug.DrawRay(shooter.position, shooter.forward, Color.red);
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
        else if (current == State.Shoot)
        {
            currentState = State.Shoot;
            StopAllCoroutines();
            StartCoroutine("AI_Shoot");
        }
    }

    IEnumerator AI_Chase()
    {
        while (currentState == State.Chase && player != null)
        {
            Debug.Log("Chasing");
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
        try
        {
            agent.SetDestination(playerT.position);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        transform.LookAt(player.transform.position);

        while (currentState == State.Attack && player != null)
        {
            Debug.Log("Attacking");
            player.GetComponent<PlayerController>().TakeDamage(damage);
            yield return new WaitForSeconds(.5f);
        }
        yield return null;
    }

    IEnumerator AI_Shoot()
    {
        try
        {
            agent.SetDestination(playerT.position);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        transform.LookAt(player.transform.position);

        while (Physics.Raycast(shooter.position, shooter.forward, out check, 20f) && player != null)
        {
            Debug.Log("Shooting");
            if (check.collider.gameObject.CompareTag("Player"))
            {
                agent.SetDestination(gameObject.transform.position);

                GameObject bulletGO = Instantiate(bullet, shooter.position, Quaternion.identity);

                bulletGO.transform.forward = shooter.forward;

                transform.LookAt(player.transform.position);
            }
            else
            {
                transform.LookAt(player.transform.position);
            }
            yield return new WaitForSeconds(.2f);

            transform.LookAt(player.transform.position);
        }

        ChangeStates(State.Chase);
        yield return null;
    }
}
