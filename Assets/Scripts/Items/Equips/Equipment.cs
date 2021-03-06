﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : Item
{
    [SerializeField] int requiredStr = 3;
    [SerializeField] int requiredDex = 3;
    [SerializeField] int requiredInt = 3;
    [SerializeField] int requiredLuk = 3;

    [SerializeField] public GameObject equippedPrefab;
    protected EquippedItems equipInventory;

    public abstract override void Action();

    protected override void UpdatePlayerData()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        inventory = player.GetComponent<PlayerInventory>();
        equipInventory = player.GetComponent<EquippedItems>();
    }
    protected abstract void RemoveCurrentEquip(GameObject weaponSlot);
}
