using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItems : MonoBehaviour
{
    Dictionary<string, int> equips = new Dictionary<string, int>();
    ItemDB itemDB;

    private void Awake()
    {
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
        equips = new Dictionary<string, int>()
        {
            { "Weapon", -1},
            { "Head", -1},
            { "Gloves", -1},
            { "Body", -1},
            { "Legs", -1},
            { "Feet", -1},
            { "Ring1", -1},
            { "Ring2", -1}
        };
    }

    private void Start()
    {
    }

    public void UpdateEquip(string type, int id)
    {
        equips[type] = id;
    }

    public Weapon GetWeapon()
    {
        return (Weapon)itemDB.GetItemByID(equips["Weapon"]);
    }

    public Equipment GetEquip(string type)
    {
        if (type.Equals("Weapon"))
        {
            return (Weapon)itemDB.GetItemByID(equips[type]);
        }

        return (Equipment)itemDB.GetItemByID(equips[type]);
    }

    public int GetEquipDamage()
    {
        if (GetWeapon() == null)
        {
            return 0;
        }
        //Once equips stats are added, more will be added to this.
        return GetWeapon().Damage;
    }

}
