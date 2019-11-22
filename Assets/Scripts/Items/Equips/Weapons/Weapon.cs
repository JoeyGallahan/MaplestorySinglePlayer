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
        UpdatePlayerData();

        Debug.Log("Equipping weapon: " + itemName);

        inventory.RemoveItem(id);
    }
}
