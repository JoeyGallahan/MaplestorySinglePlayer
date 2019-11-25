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
            GameObject item = db.GetItemByID(i).DropPrefab;
            Instantiate(item, position, Quaternion.identity);
            item.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -25.0f), ForceMode2D.Impulse);
        }
    }
}
