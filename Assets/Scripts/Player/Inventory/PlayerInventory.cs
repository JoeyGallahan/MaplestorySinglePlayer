using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
    private static PlayerInventory instance = null;
    private static readonly object padlock = new object();

    PlayerInventory() { }

    public static PlayerInventory Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new PlayerInventory();
                }
                return instance;
            }
        }
    }

    Dictionary<int, int> itemIDsAndAmount = new Dictionary<int,int>();
    InventoryUI inventoryUI;
    ItemDB db;

    public InventoryUI UI { get => inventoryUI; }
    public bool Opened { get => inventoryUI.Showing(); }

    private void Awake()
    {
        instance = this;

        inventoryUI = GameObject.FindGameObjectWithTag("InventoryCanvas").GetComponentInChildren<InventoryUI>();

        db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
    }

    //Returns whether or not an item actually exists in your inventory
    public bool ContainsID(int id)
    {
        return itemIDsAndAmount.ContainsKey(id);
    }

    //Gets the amount of a certain item you have (eg: 5 red potions)
    public int GetAmountByID(int id)
    {
        return itemIDsAndAmount[id];
    }

    //Adds an item to your inventory
    public void AddToInventory(int id, int amount = 1)
    {
        if(itemIDsAndAmount.ContainsKey(id))
        {
            itemIDsAndAmount[id] += amount;
            inventoryUI.UpdateGridItemAmount(id, itemIDsAndAmount[id]);
        }
        else
        {
            itemIDsAndAmount.Add(id, amount);
            inventoryUI.AddToGrid(id, amount);
        }
    }

    //Prints out all of the items in the dictionary incase there are any issues with the UI
    public void DebugInv()
    {
        string inventory = "";

        foreach(KeyValuePair<int, int> item in itemIDsAndAmount)
        {
            inventory += ("Key: {0}, Value: {1}", item.Key, item.Value);
        }

        Debug.Log(inventory);
    }

    //Removes an item from your inventory. Defaulted to removing 1, but can be customized for dropping more
    public void RemoveItem(int id, int amount = 1)
    {
        //If this item exists in the inventory
        if (itemIDsAndAmount.ContainsKey(id))
        {
            itemIDsAndAmount[id] -= amount; //Decrease the amount of this item
            inventoryUI.UpdateGridItemAmount(id, itemIDsAndAmount[id]); //Update the inventory UI

            //If you've used all of this item
            if (itemIDsAndAmount[id] <= 0)
            {
                itemIDsAndAmount.Remove(id); //Remove it from your inventory completely.
            }
        }
    }
}
