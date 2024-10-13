using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    public GameObject enemyPrefab;
    public Transform spawnLocation;

     public Transform player;    

    public void EnemyDied()
    {
        StartCoroutine(RespawnEnemy(2.0f));
        StartCoroutine(RespawnEnemy(4.0f));
    }

    private IEnumerator RespawnEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(enemyPrefab, spawnLocation.position, spawnLocation.rotation);

        BigEnemies bigEnemyScript = FindObjectOfType<BigEnemies>();
        if (bigEnemyScript != null)
        {
            bigEnemyScript.InitNavMesh();  
            bigEnemyScript.setPlayer(player);  
        }
    }
}

