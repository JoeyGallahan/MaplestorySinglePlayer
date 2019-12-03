using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public string classType = "Warrior";
    public int str = 3, dex = 3, intel = 3, luk = 3;

    public CharacterData(TempCharacter temp)
    {
        //AP
        str = (int)temp.str;
        dex = (int)temp.dex;
        intel = (int)temp.intel;
        luk = (int)temp.luk;

        //Info
        characterName = temp.characterName;
        classType = temp.classType;
    }
}
