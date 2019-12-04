using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    GameObject parentObj;

    InventoryGrid equipGrid, useGrid, etcGrid;
    ItemDB itemDB;
    [SerializeField] GameObject equipParent, useParent, etcParent;

    private void Awake()
    {
        parentObj = GameObject.FindGameObjectWithTag("InventoryCanvas");

        equipGrid = GameObject.FindGameObjectWithTag("EquipGrid").GetComponent<InventoryGrid>();
        useGrid   = GameObject.FindGameObjectWithTag("UseGrid").GetComponent<InventoryGrid>();
        etcGrid   = GameObject.FindGameObjectWithTag("EtcGrid").GetComponent<InventoryGrid>();

        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Show(false);
        SwitchToEquip();
    }

    public void Show(bool maybe)
    {
        parentObj.SetActive(maybe);
    }
    public bool Showing()
    {
        return parentObj.activeInHierarchy;
    }

    public void SwitchToEquip()
    {
        equipParent.SetActive(true);
        useParent.SetActive(false);
        etcParent.SetActive(false);
    }

    public void SwitchToUse()
    {
        equipParent.SetActive(false);
        useParent.SetActive(true);
        etcParent.SetActive(false);
    }

    public void SwitchToEtc()
    {
        equipParent.SetActive(false);
        useParent.SetActive(false);
        etcParent.SetActive(true);
    }

    public void AddToGrid(int id)
    {
        Item item = itemDB.GetItemByID(id);

        switch (item.Category)
        {
            case Item.ItemCategory.EQUIPMENT:
                equipGrid.AddToGrid(item.UIPrefab);
                break;
            case Item.ItemCategory.USE:
                useGrid.AddToGrid(item.UIPrefab);
                break;
            case Item.ItemCategory.ETC:
                etcGrid.AddToGrid(item.UIPrefab);
                break;
        }
    }

    public void UpdateGridItemAmount(int id, int amount)
    {
        Item item = itemDB.GetItemByID(id);
               
        switch (item.Category)
        {
            case Item.ItemCategory.EQUIPMENT:
                equipGrid.UpdateGridItemAmount(id, amount);
                break;
            case Item.ItemCategory.USE:
                useGrid.UpdateGridItemAmount(id, amount);
                break;
            case Item.ItemCategory.ETC:
                etcGrid.UpdateGridItemAmount(id, amount);
                break;
        }
    }
}
