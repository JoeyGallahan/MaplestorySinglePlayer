using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public enum ItemCategory
    {
        EQUIPMENT = 0,
        USE = 1,
        ETC = 2
    }

    [SerializeField] protected int id;
    [SerializeField] protected ItemCategory itemCategory;
    [SerializeField] protected string itemName;
    [SerializeField] protected int sellPrice;
    [SerializeField] protected float dropChance;
    [SerializeField] protected GameObject uiPrefab;
    [SerializeField] protected GameObject dropPrefab;

    protected PlayerCharacter player;
    protected PlayerInventory inventory;

    public GameObject DropPrefab { get => dropPrefab; }
    public int ID { get => id; }
    public string ItemName { get => itemName; }
    public GameObject UIPrefab { get => uiPrefab; }
    public ItemCategory Category { get => itemCategory; }
    public float DropChance { get => dropChance; }

    public abstract void Action();
    protected virtual void UpdatePlayerData()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }
}
