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

    public override void Action()
    {
        UpdatePlayerData(); //Gets the updated information of the player.

        Debug.Log("Equipping weapon: " + itemName);

        GameObject weaponSlot = GameObject.FindGameObjectWithTag("Weapon");

        GameObject newWeapon = (GameObject)Instantiate(equippedPrefab, weaponSlot.transform);

        player.UpdateEquip("Weapon", id);

        inventory.RemoveItem(id);
    }
}
