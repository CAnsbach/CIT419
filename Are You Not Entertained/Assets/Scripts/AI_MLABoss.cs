using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class AI_MLABoss : Agent
{
    
    [SerializeField] private Transform target;                  //Target to attack
    [SerializeField] private Material win;                      //Materials used to display success and failure
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
        //If there is no enemy, there is no enemy in range
        if (enemy == null)
        {
            enemyInRange = false;
        }

        //If it cannot attack and the attack cooldown is done, it can attack again
        if (!canAttack && Time.time > attackTracker)
        {
            canAttack = true;
        }

        //If it cannot block and the block cooldown is done, it can block again
        if (!canBlock && Time.time > stopWatch)
        {
            canBlock = true;
        }

        //If it is currently blocking and the block duraiton is done, it is no longer blocking
        if (blocking && Time.time > blockStop)
        {
            blocking = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Determine the tag of the object that entared and act accordingly
        if (other.gameObject.CompareTag("TestEnemy") && training)
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
        //Determine the tag of the object that left and act accordingly
        if (other.gameObject.CompareTag("TestEnemy") && training)
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

    /// <summary>
    /// Method used when training the AI and an episode has restarted
    /// </summary>
    public override void OnEpisodeBegin()
    {
        SetReward(0f);              //Reset the reward
        health = 100;               //Reset health
        //If the AI is training, reset it's position and start the next training wave
        if (training)
        {
            
            transform.position = new Vector3(-43.6f, 0.5f, 41.4f);
            gc.NextTrainingWave();
        } 
    }

    /// <summary>
    /// Method used to collect observations from the AI's environment
    /// </summary>
    /// <param name="sensor">sensor used to collect observations</param>
    public override void CollectObservations(VectorSensor sensor)
    {
        //If the AI is training and the target is null, assign a new target
        if (training && target == null)
        {
            target = GameObject.FindGameObjectsWithTag("TestEnemy")[0].transform;

            //Add the target's transform as an observation
            sensor.AddObservation(target.position);
        }
        else
        {
            sensor.AddObservation(target.position);
        }
        //Add the AI's transform and booleans to keep track of cooldown and enemies to the observations
        sensor.AddObservation(transform.position);
        sensor.AddObservation(enemyInRange);
        sensor.AddObservation(canAttack);
        sensor.AddObservation(canBlock);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int chase = actions.DiscreteActions[0];     //Action to take for chasing
        int attack = actions.DiscreteActions[1];    //Action to take for attacking
        int block = actions.DiscreteActions[2];     //Action to take for blocking

        //If the action chosen for chasing is 1, chase the target.
        if (chase == 1)
        {
            Debug.Log("Chasing");
            Chase();
        }
        
        //If the action chosen for attacking is 1, try to attack.
        if (attack == 1)
        {
            Debug.Log("Attack");
            Attack();
        }
        
        //If the action chosen for blocking is 1, try to block.
        if (block == 1)
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
                    //Determine what is hit and act accordingly
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
                AddReward(10f);     //Reward for attacking an enemy
            }
        }

        //Punish for trying to attack on cooldown
        else
        {
            AddReward(-.3f);
        }
    }

    /// <summary>
    /// Method used to block
    /// </summary>
    void Block()
    {
        //If the AI can block, do so
        if (canBlock)
        {
            canBlock = false;
            blocking = true;

            stopWatch = Time.time + blockCooldown;
            blockStop = Time.time + blockDuration;
        }
        //Else, punish
        else
        {
            AddReward(-.1f);
        }
    }

    /// <summary>
    /// Method used to chase the target
    /// </summary>
    void Chase()
    {
        //If there is a target to chase, move towards it
        if (target != null)
        {
            transform.LookAt(target.position);

            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Method used to have the AI take damage
    /// </summary>
    /// <param name="damage">Damage for the AI to take</param>
    public void Hit(int damage)
    {
        //If the AI is not blocking, damage them
        if (!blocking)
        {
            if (damage < health)
            {
                health -= damage;       //Update health
                AddReward(-.1f);        //Small punishment
            }
            else
            {
                health = 0;
                Die();
            }
        }
        //Large reward for successfully blocking damage
        else
        {
            AddReward(5f);
        }
    }

    /// <summary>
    /// Method used to kill the AI
    /// </summary>
    void Die()
    {
        //If the AI is in training, destroy remaining enemies and set the materials accordingly
        if (training)
        {
            foreach (GameObject enemies in GameObject.FindGameObjectsWithTag("TestEnemy"))
            {
                Destroy(enemies);
            }
            floorMeshRenderer.material = lose;
            SetReward(-100f);
            Debug.Log(GetCumulativeReward());
            gc.RemoveSelf(gameObject);
            EndEpisode();
            Destroy(gameObject);
        }

        //Else, punish the AI heavily, update score, remove the AI from the list of enemies, end the episode, and destroy the game object
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

    /// <summary>
    /// Method used during training for when the AI wins
    /// </summary>
    void EnemyKilled()
    {
        //If the AI is training, set the material accordingly, provide a large reward, remove remaining enemies, and end the episode
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

    /// <summary>
    /// Method used to check for enemies
    /// </summary>
    /// <returns>Boolean stating whether there are more enemies or not</returns>
    bool CheckForEnemies()
    {
        return GameObject.FindGameObjectsWithTag("TestEnemy").Length > 0;
    }
}
