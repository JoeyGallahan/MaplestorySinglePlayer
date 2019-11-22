using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();

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
