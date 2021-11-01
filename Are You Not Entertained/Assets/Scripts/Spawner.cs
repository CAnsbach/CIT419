using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;

    /// <summary>
    /// Method used ot spawn the assigned enemy
    /// </summary>
    public void SpawnEnemy()
    {
        Instantiate(enemy, transform);
    }
}
