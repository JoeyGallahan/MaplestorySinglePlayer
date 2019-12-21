using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryGrid : MonoBehaviour
{
    public void AddToGrid(GameObject prefab, int amount)
    {
        GameObject newObj = (GameObject)Instantiate(prefab, transform);

        if (amount > 1)
        {
            TextMeshProUGUI itemText = newObj.GetComponentInChildren<TextMeshProUGUI>();
            itemText.SetText(amount.ToString());
        }
    }

    public void UpdateGridItemAmount(int id, int amount)
    {
        ItemID[] itemIDs = GetComponentsInChildren<ItemID>();

        TextMeshProUGUI itemText = new TextMeshProUGUI();

        foreach (ItemID i in itemIDs)
        {
            if (i.itemID == id)
            {
                itemText = i.GetComponentInChildren<TextMeshProUGUI>();
                               
                if (amount <= 0)
                {
                    Destroy(i.gameObject);
                    return;
                }

                break;
            }
        }
        itemText.SetText(amount.ToString());
    }
}
