using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    ItemDB itemDB;

    private void Awake()
    {
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEquip(int id)
    {
        GameObject newEquip;
        Item equip = itemDB.GetItemByID(id);

        newEquip = (GameObject)Instantiate(equip.UIPrefab, transform);
        newEquip.transform.parent = transform;
        newEquip.transform.localScale = new Vector3(1.0f, 1.0f);
    }
}
