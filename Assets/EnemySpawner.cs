using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> shinyEnemies = new List<GameObject>();
    public float spawnTime = 5;
    public float minSpawnTime = 1;
    public float copy = 1;

    void Start()
    {
        List<GameObject> list = enemies;
        for (int i = 1; i < copy; i++)
        {
            enemies.AddRange(list);
        }
        foreach (GameObject se in shinyEnemies)
        {
            enemies.Add(se);
        }
        Invoke("Spawn", spawnTime);
    }

    void Spawn()
    {
        int z = Random.Range(0, enemies.Count);
        Instantiate(enemies[z], new Vector3(9.5f, Random.Range(3.7f, -3.7f)), enemies[z].transform.rotation);
        spawnTime -= spawnTime >= minSpawnTime ? 0.02f : 0;
        CancelInvoke("Spawn");
        Invoke("Spawn", spawnTime);
    }
}
