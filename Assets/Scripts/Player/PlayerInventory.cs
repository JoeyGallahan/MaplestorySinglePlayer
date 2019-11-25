using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] Dictionary<int, int> itemIDsAndAmount = new Dictionary<int,int>();
    InventoryUI inventoryUI;
    ItemDB db;

    public InventoryUI UI { get => inventoryUI; }
    public bool Opened { get => inventoryUI.Showing(); }

    private void Awake()
    {
        inventoryUI = GameObject.FindGameObjectWithTag("InventoryCanvas").GetComponentInChildren<InventoryUI>();

        db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
    }

    public void AddToInventory(int id)
    {
        if(itemIDsAndAmount.ContainsKey(id))
        {
            itemIDsAndAmount[id]++;
            inventoryUI.UpdateGridItemAmount(id, itemIDsAndAmount[id]);
        }
        else
        {
            itemIDsAndAmount.Add(id,1);
            inventoryUI.AddToGrid(id);
        }
    }

    public void DebugInv()
    {
        string inventory = "";

        foreach(KeyValuePair<int, int> item in itemIDsAndAmount)
        {
            inventory += ("Key: {0}, Value: {1}", item.Key, item.Value);
        }

        Debug.Log(inventory);
    }

    public void RemoveItem(int id, int amount = 1)
    {
        if (itemIDsAndAmount.ContainsKey(id))
        {
            itemIDsAndAmount[id] -= amount;
            inventoryUI.UpdateGridItemAmount(id, itemIDsAndAmount[id]);

            if (itemIDsAndAmount[id] <= 0)
            {
                itemIDsAndAmount.Remove(id);
            }
        }
    }
}
