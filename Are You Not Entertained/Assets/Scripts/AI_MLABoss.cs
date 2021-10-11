using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AI_MLABoss : Agent
{
    [SerializeField] private Transform target;
    
    public enum State { Chase, Attack };
    State currentState;

    public GameController gc;
    int health = 150, score = 100;
    const int damage = 10;
    bool canBlock = true;
    bool blocking = false;
    const float blockCooldown = 5f, blockDuration = 3f;
    float stopWatch, blockStop, x, z, currentDistance;
    bool enemyInRange;
    GameObject enemy;

    private void Update()
    {
        if (!canBlock && Time.time > stopWatch)
        {
            canBlock = true;
            Debug.Log("Can Block Again");
        }

        if (blocking && Time.time > blockStop)
        {
            blocking = false;
            Debug.Log("No longer blocking");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TestEnemy"))
        {
            enemyInRange = true;
            enemy = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TestEnemy"))
        {
            enemyInRange = false;
            enemy = null;
        }
    }

    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(-4.34f, 1f, 0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(target.position);
        sensor.AddObservation(enemyInRange);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        x = actions.ContinuousActions[0];
        z = actions.ContinuousActions[1];

        Debug.Log(x + ", " + z);

        currentDistance = Vector3.Distance(transform.position, target.position);

        int state = actions.DiscreteActions[0];

        if (state == 0)
        {

            Debug.Log("Chasing");
            Chase();
        }
        else if (state == 1)
        {
            Debug.Log("Attack");
            Attack();
        }
        else if (state == 2)
        {
            Debug.Log("Block");
            Block();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuous = actionsOut.ContinuousActions;
        continuous[0] = Input.GetAxisRaw("Horizontal");
        continuous[1] = Input.GetAxisRaw("Vertical");

        //ActionSegment<int> discrete = actionsOut.DiscreteActions;
    }



    void Attack()
    {
        //Attack in front, punish if nothing there, reward on hit.
        //If there is no enemy in range, punish
        if (!enemyInRange)
        {
            AddReward(-10f);
        }

        else
        {
            //If the enemy is an enemy, hit them and check if they are still alive
            if (enemy.CompareTag("TestEnemy"))
            {
                enemy.GetComponent<AI_BasicMelee>().Hit(damage);

                //If they are not alive, reward the AI and end the episode.
                if (CheckForEnemies())
                {
                    EnemyKilled();
                }
            }
            AddReward(10f);
        }
    }

    void Chase()
    {
        //Chase the enemy, punish if moving away, reward if getting closer.
        float moveSpeed = 1;

        transform.position += new Vector3(x, 0, z) * Time.deltaTime * moveSpeed;

        //If you are moving away, punish
        if (Vector3.Distance(transform.position, target.position) > currentDistance)
        {
            AddReward(-1f);
        }
        //Else, reward
        else
        {
            AddReward(1f);
        }
    }

    public void Hit(int damage)
    {
        if (!blocking)
        {
            if (damage < health)
            {
                health -= damage;
                AddReward(-5f);
            }
            else
            {
                health = 0;
                Die();
            }
        }

        else
        {
            AddReward(5f);
            Debug.Log("Blocked Damage");
        }
    }

    void Die()
    {
        Debug.Log("Dieing");
        SetReward(-100f);
        EndEpisode();

        //gc.UpdateScore(score);
        //Debug.Log("Score is now: " + gc.GetScore());
        //Destroy(gameObject);
    }

    void EnemyKilled()
    {
        SetReward(100f);
        EndEpisode();
    }

    void Block()
    {
        if (canBlock)
        {
            canBlock = false;
            blocking = true;

            stopWatch = Time.time + blockCooldown;
            blockStop = Time.time + blockDuration;
        }
        else
        {
            AddReward(-10f);
        }
    }

    bool CheckForEnemies()
    {
        return GameObject.FindGameObjectsWithTag("TestEnemy").Length > 0;
    }
}
