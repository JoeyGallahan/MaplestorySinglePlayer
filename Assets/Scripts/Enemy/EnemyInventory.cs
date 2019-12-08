using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInventory : MonoBehaviour
{
    [SerializeField]List<int> itemIds = new List<int>();
    
    public void DropItems(Vector2 position)
    {
        ItemDB db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();

        foreach(int i in itemIds)
        {
            GameObject item = (GameObject)Instantiate(db.GetItemByID(i).DropPrefab, position, Quaternion.identity);

            float randomXForce = Random.Range(-5.0f, 5.0f);

            //Max them seem like they kind of burst out of the enemy
            item.GetComponent<PhysicsObject>().AddForce(new Vector2(randomXForce, 5.0f) * Time.deltaTime);
        }
    }
}
