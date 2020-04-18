using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ItemDB : MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();
    private static ItemDB instance = null;
    private static readonly object padlock = new object();

    ItemDB(){}

    private void Awake()
    {
        instance = this;
    }

    public static ItemDB Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new ItemDB();
                }
                return instance;
            }
        }
    }

    public Item GetItemByID(int id)
    {
        foreach(Item i in items)
        {
            if (i.ID == id)
            {
                return i;
            }
        }

        return null;
    }

    public string GetItemName(int id)
    {
        foreach (Item i in items)
        {
            if (i.ID == id)
            {
                return i.ItemName;
            }
        }

        return "";
    }

    public GameObject GetSprite(int id)
    {
        foreach (Item i in items)
        {
            if (i.ID == id)
            {
                return i.UIPrefab;
            }
        }

        return null;
    }
}
