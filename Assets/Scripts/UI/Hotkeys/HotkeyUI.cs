using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HotkeyUI : MonoBehaviour
{
    Hotkey key; //The information attached to the hotkey

    //Items
    TextMeshProUGUI itemAmountText; //The item amount in the hotkey if relevant
    PlayerInventory inventory; //The inventory of the player

    private void Awake()
    {
        key = GetComponentInChildren<Hotkey>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    private void LateUpdate()
    {
        UpdateKeyItemAmount(); //We want to check to see if the item numbers have changed at all during this frame, so we want to have it in late update.
    }

    //Adds an item or skill to the Hotkey
    public void AddToKey(int id, Hotkey.HotkeyType type)
    {
        //If there was already something in this slot, destroy it
        if (key.hotkeyType != Hotkey.HotkeyType.EMPTY)
        {
            EmptySlot();
        }

        key.UpdateHotkey(id, type); //Updates the hotkey with the type (skill or item) and the id

        //If you're adding an item to this slot, we want to store the text so we can update it later
        if (type.Equals(Hotkey.HotkeyType.ITEM))
        {
            itemAmountText = GetComponentInChildren<ItemID>().GetComponentInChildren<TextMeshProUGUI>();
        }
        else //otherwise, it's a skill and we set it to null
        {
            itemAmountText = null;
        }
    }

    //Updates the UI text for the item in your hotkey slot
    public void UpdateKeyItemAmount()
    {
        //If there is actually an item in this slot
        if (itemAmountText != null)
        {
            //Check to see if the inventory still contains an item with this id (might have used all of them)
            if (inventory.ContainsID(key.id))
            {
                //If the inventory says you have a different amount of this item than the hotkey says
                if (inventory.GetAmountByID(key.id).ToString() != itemAmountText.text)
                {
                    itemAmountText.SetText(inventory.GetAmountByID(key.id).ToString()); //update the UI text to match
                }
            }
            else //If this item is not in your inventory, remove it from the hotkey slot
            {
                EmptySlot();
            }
        }
    }

    private void EmptySlot()
    {
        GameObject toDestroy;

        //If it's an item, we want to go off the ItemID to get the proper gameobject to destroy
        if (key.hotkeyType == Hotkey.HotkeyType.ITEM)
        {
            toDestroy = GetComponentInChildren<ItemID>().gameObject;
            itemAmountText = null;
        }
        else
        {
            toDestroy = GetComponentInChildren<SkillID>().gameObject;
        }

        //Reset the values of the hotkey
        key.hotkeyType = Hotkey.HotkeyType.EMPTY;
        key.id = -1;

        Destroy(toDestroy); //BOom remove it from the UI
    }
}
