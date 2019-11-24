using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//--------Used for Character Creator---------//
[System.Serializable]
public class TempCharacter : MonoBehaviour
{
    public string classType = "Warrior";
    public int str = 0, dex = 0, intel = 0, luk = 0;
    public int remainingAP = 5;
    public int rollAP = 7;
    public string characterName;
    public bool rolled = false;

    public void updateAP(string apType, int amount)
    {
        switch (apType)
        {
            case "STR": str += amount;
                break;
            case "DEX": dex += amount;
                break;
            case "INT": intel += amount;
                break;
            case "LUK": luk += amount;
                break;
        }
    }

    public void updateClassType(string type)
    {
        classType = type;
    }

    public void SaveCharacter()
    {
        CharacterCreationSave.SaveCharacter(this);

        SceneManager.LoadScene("SampleScene");
    }
}