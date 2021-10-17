using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class AI_MLABoss : Agent
{
    
    [SerializeField] private Transform target;
    [SerializeField] private Material win;
    [SerializeField] private Material lose;
    [SerializeField] private MeshRenderer floorMeshRenderer;

    public GameController gc;
    public int health = 100, score = 100;
    public bool training;

    public GameObject enemy;

    public const int damage = 10;
    public const float blockCooldown = 5f, blockDuration = 3f, moveSpeed = 5f, attackCooldown = 1f;

    public bool canBlock = true, blocking = false, canAttack = true, enemyInRange;
    public float stopWatch, blockStop, attackTracker;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        training = SceneManager.GetActiveScene().name == "MLTraining";
        gc.AddSelf(gameObject);
    }

    private void Update()
    {
        if (enemy == null)
        {
            enemyInRange = false;
        }

        if (!canAttack && Time.time > attackTracker)
        {
            canAttack = true;
            Debug.Log("Can attack again");
        }

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

        else if (other.gameObject.CompareTag("Player"))
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

        else if (other.gameObject.CompareTag("Player"))
        {
            enemyInRange = true;
            enemy = null;
        }
    }

    public override void OnEpisodeBegin()
    {
        SetReward(0f);
        health = 100;
        if (training)
        {
            
            transform.position = new Vector3(-43.6f, 0.5f, 41.4f);
            gc.NextTrainingWave();
        } 
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (training && target == null)
        {
            target = GameObject.FindGameObjectsWithTag("TestEnemy")[0].transform;

            sensor.AddObservation(target.position);
        }
        else
        {
            sensor.AddObservation(target.position);
        }
        sensor.AddObservation(transform.position);
        sensor.AddObservation(enemyInRange);
        sensor.AddObservation(canAttack);
        sensor.AddObservation(canBlock);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int chase = actions.DiscreteActions[0];
        int attack = actions.DiscreteActions[1];
        int block = actions.DiscreteActions[2];

        if (chase == 1)
        {
            Debug.Log("Chasing");
            Chase();
        }
        else if (attack == 1)
        {
            Debug.Log("Attack");
            Attack();
        }
        else if (block == 1)
        {
            Debug.Log("Block");
            Block();
        }
    }

    void Attack()
    {
        //Attack in front, punish if nothing there, reward on hit.
        if (canAttack)
        {
            //If there is no enemy in range, punish
            if (!enemyInRange || enemy == null)
            {
                AddReward(-.5f);
            }

            else
            {
                //If the enemy is an enemy, hit them and check if they are still alive
                if (enemy.CompareTag("Player") || enemy.CompareTag("TestEnemy"))
                {
                    if(enemy.GetComponent<AI_BasicRanged>() != null)
                    {
                        enemy.GetComponent<AI_BasicRanged>().Hit(damage);
                        canAttack = false;

                        attackTracker = Time.time + attackCooldown;

                    }
                    else if (enemy.GetComponent<AI_BasicMelee>() != null)
                    {
                        enemy.GetComponent<AI_BasicMelee>().Hit(damage);
                        canAttack = false;

                        attackTracker = Time.time + attackCooldown;
                    }
                    else
                    {
                        enemy.GetComponent<PlayerController>().TakeDamage(damage);
                        canAttack = false;

                        attackTracker = Time.time + attackCooldown;
                    }
                    

                    //If they are not alive, reward the AI and end the episode.
                    if (training)
                    {
                        if (CheckForEnemies())
                        {
                            EnemyKilled();
                        }
                        else
                        {
                            target = GameObject.FindGameObjectsWithTag("TestEnemy")[0].transform;
                        }
                    }
                    
                }
                AddReward(10f);
            }
        }

        else
        {
            AddReward(-.3f);
        }
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
            AddReward(-.1f);
        }
    }

    void Chase()
    {
        if (target != null)
        {
            transform.LookAt(target.position);

            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    public void Hit(int damage)
    {
        if (!blocking)
        {
            if (damage < health)
            {
                health -= damage;
                AddReward(-.1f);
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
        if (training)
        {
            foreach (GameObject enemies in GameObject.FindGameObjectsWithTag("TestEnemy"))
            {
                Destroy(enemies);
            }

            Debug.Log("Dieing");
            floorMeshRenderer.material = lose;

        }


        else
        {
            SetReward(-100f);
            Debug.Log(GetCumulativeReward());
            gc.UpdateScore(score);
            gc.RemoveSelf(gameObject);
            EndEpisode();
            Destroy(gameObject);
        }
    }

    void EnemyKilled()
    {
        if (training)
        {
            Debug.Log("Enemy Killed :)");
            floorMeshRenderer.material = win;
            SetReward(100f);
            Debug.Log(GetCumulativeReward());
            foreach (GameObject enemies in GameObject.FindGameObjectsWithTag("TestEnemy"))
            {
                Destroy(enemies);
            }

            EndEpisode();        
        }
    }

    bool CheckForEnemies()
    {
        return GameObject.FindGameObjectsWithTag("TestEnemy").Length > 0;
    }
}
