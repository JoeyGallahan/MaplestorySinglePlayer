using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] int maxNumEnemies = 10;
    [SerializeField] int curNumEnemies = 0;

    [SerializeField] List<GameObject> enemyList = new List<GameObject>(); //The list of current enemies on the map
    [SerializeField] List<GameObject> possibleEnemyPrefabs = new List<GameObject>(); //The list of possible enemy types that could spawn

    [SerializeField] List<GameObject> spawnPoints; //The locations that enemies can spawn

    private void Start()
    {
        spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("SpawnPoint"));
    }

    private void Update()
    {
        curNumEnemies = enemyList.Count;

        if (curNumEnemies < maxNumEnemies)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int enemyIndex = (int)Random.Range(0, possibleEnemyPrefabs.Count); //Get a random enemy type
        int spawnLocIndex = (int)Random.Range(0, spawnPoints.Count); //Get a random spawn point

        BoxCollider2D spawnArea = spawnPoints[spawnLocIndex].GetComponent<BoxCollider2D>(); //Grab the box collider from the spawn point
        Vector3 min = spawnArea.bounds.min; //Get the min bounds of our spawn point
        Vector3 max = spawnArea.bounds.max; //Get the max bounds of our spawn point

        Vector3 spawnLocation = Vector3.zero; 
        spawnLocation.z = gameObject.transform.position.z; //Set the z position to whatever it would normally be
        spawnLocation.x = Random.Range(min.x, max.x); //Get a random x position between the min and max bounds
        spawnLocation.y = min.y; //Get the y position of the spawn point

        GameObject newEnemy = (GameObject)Instantiate(possibleEnemyPrefabs[enemyIndex]); //Actually spawns the enemy
        newEnemy.transform.SetParent(this.transform); //Puts this new spawn under the correct parent. Just for hierarchy reasons
        newEnemy.transform.position = spawnLocation; //Move it to the random spawn location we just created 

        enemyList.Add(newEnemy); //Add this new enemy to our list
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemyList.Remove(enemy);
    }
}
