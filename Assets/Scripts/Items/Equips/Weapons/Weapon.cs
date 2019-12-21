using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment/Weapon", fileName = "Weapon.asset")]
public class Weapon : Equipment
{
    [SerializeField] WeaponType weaponType;
    [SerializeField] int damage;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;

    public int Damage { get => damage; }
    public float AttackRange { get => attackRange; }
    public float AttackSpeed { get => attackSpeed; }
    public WeaponType WeaponStyle { get => weaponType; }

    public override void Action()
    {
        UpdatePlayerData(); //Gets the updated information of the player.        

        GameObject weaponSlot = GameObject.FindGameObjectWithTag("Weapon");
        RemoveCurrentEquip(weaponSlot);

        GameObject newWeapon = (GameObject)Instantiate(equippedPrefab, weaponSlot.transform);
                
        Debug.Log("Equipping weapon: " + itemName);
        player.UpdateEquip("Weapon", id);

        inventory.RemoveItem(id);
    }

    protected override void RemoveCurrentEquip(GameObject weaponSlot)
    {
        ItemID id = weaponSlot.GetComponentInChildren<ItemID>();

        if (id != null)
        {
            int ID = id.itemID;
            Debug.Log(ID);
            Destroy(id.gameObject);
            player.ui.RemoveEquip("Weapon");
            inventory.AddToInventory(ID);
        }
    }
}
