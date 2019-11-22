using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//--------Used for Character Creator---------//
[System.Serializable]
public class TempCharacter : MonoBehaviour
{
    public string classType = "Warrior";
    public float str = 3.0f, dex = 3.0f, intel = 3.0f, luk = 3.0f;
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