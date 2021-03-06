﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateHotkey : MonoBehaviour
{
    [SerializeField] KeyCode keyPress;
    Hotkey hotkey;
    ItemDB itemDB;
    SkillDB skillDB;

    private void Awake()
    {
        hotkey = GetComponent<Hotkey>();
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
        skillDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<SkillDB>();
    }

    // Update is called once per frame
    void Update()
    {
        //If you press the button associated with the hotkey slot
        if (Input.GetKeyDown(keyPress))
        {
            int id = hotkey.id; //Get the id of the item or skill the player is going to use
            if (id != -1) //If there's actually something in the hotkey slot
            {
                if (hotkey.hotkeyType.Equals(Hotkey.HotkeyType.ITEM)) //If it's an item
                {
                    UsableItem item = (UsableItem)itemDB.GetItemByID(id); //Get the actual item
                    item.Action(); //Perform the action associated with the item
                }
                else //If it's a skill
                {                   
                    Skill skill = (Skill)skillDB.GetSkillByID(id); //Get the actual skill
                    skill.UseSkill(); //Use the skill
                }
            }
        }

    }
}
