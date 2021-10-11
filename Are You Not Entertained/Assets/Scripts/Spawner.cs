using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;

    public void SpawnEnemy()
    {
        Instantiate(enemy, transform);
    }
}
