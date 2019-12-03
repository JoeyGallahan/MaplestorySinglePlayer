using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotkey : MonoBehaviour
{
    public enum HotkeyType
    {
        EMPTY = -1,
        ITEM,
        SKILL
    }

    public HotkeyType hotkeyType = HotkeyType.EMPTY; //The type of hotkey this will be
    public int id = -1; //The id of the item or skill in the hotkey
    
    //Updates the information of the hotkey so we can use it elsewhere
    public void UpdateHotkey(int keyID, HotkeyType type)
    {
        hotkeyType = type;
        id = keyID;
    }

}
