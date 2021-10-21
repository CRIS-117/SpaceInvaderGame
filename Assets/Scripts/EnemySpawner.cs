using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs, waypointsPrefabs;
    [SerializeField] float delayTime;

    int waveIndex, enemyCounter;
    float spawnTime;
    
    void Start()
    {
        waveIndex = 1;
        StartCoroutine(StartEnemyCoroutines());
    }

    void CreateEnemy(GameObject prefab, float positionX, int waypoints = 0)
    {
        Vector2 enemyPosition = new Vector2(positionX, 11f);
        GameObject newEnemy = Instantiate(prefab, enemyPosition, Quaternion.identity);
        newEnemy.transform.SetParent(FindObjectOfType<Instances>().enemies);
        if(newEnemy.GetComponent<Enemy3>())
        {
            switch(waypoints)
            {
                case 1: newEnemy.GetComponent<Enemy3>().SetPath(waypointsPrefabs[0]);break;
                case 2: newEnemy.GetComponent<Enemy3>().SetPath(waypointsPrefabs[1]);break;
            }
        }
    }

    IEnumerator StartEnemyCoroutines()
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(SpawnEnemies1());
        StartCoroutine(SpawnEnemies2());
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnEnemies1()
    {
        yield return new WaitForSeconds(2f);
        CreateEnemy(enemyPrefabs[0], Random.Range(-2f, 2f));
        StartCoroutine(SpawnEnemies1()); 
    }

    IEnumerator SpawnEnemies2()
    {
        yield return new WaitForSeconds(4f);
        CreateEnemy(enemyPrefabs[1], Random.Range(-1f, 1f));
        StartCoroutine(SpawnEnemies2()); 
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(5f);
        switch(waveIndex)
        {
            case 1: enemyCounter = 3; spawnTime = .3f; break;
            case 2: enemyCounter = 5; spawnTime = .5f; break;
            case 3: enemyCounter = 3; spawnTime = .3f; break;
            case 4: enemyCounter = 5; spawnTime = .5f; break;
        }
        StartCoroutine(SpawnWaves());
        StartCoroutine(SpawnWaveEnemy());
    }

    IEnumerator SpawnWaveEnemy()
    {
        yield return new WaitForSeconds(spawnTime);
        float positionX = 0;
        switch(waveIndex)
        {
            case 1: positionX = -4; CreateEnemy(enemyPrefabs[2], positionX, 1); break;
            case 2: positionX = Random.Range(-5f, 5f); CreateEnemy(enemyPrefabs[3], positionX);break;
            case 3: positionX = 4; CreateEnemy(enemyPrefabs[2], positionX, 1); break;
            case 4: positionX = Random.Range(-5f, 5f); CreateEnemy(enemyPrefabs[3], positionX);break;

        }
        enemyCounter--;
        if(enemyCounter == 0)
        {
            waveIndex++;
            if (waveIndex > 4)
            waveIndex = 1;
        }
        else 
        {
            StartCoroutine(SpawnWaveEnemy());
        }
    }
    
}
