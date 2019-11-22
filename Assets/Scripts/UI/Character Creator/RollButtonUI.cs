using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollButtonUI : MonoBehaviour
{
    Button btn;
    TempCharacter character;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("GameController").GetComponent<TempCharacter>();

        btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        List<string> ap = new List<string>()
        {
            "STR", "DEX", "INT", "LUK"
        };

        character.str = 3.0f;
        character.dex = 3.0f;
        character.intel = 3.0f;
        character.luk = 3.0f;

        while (ap.Count > 0 && character.rollAP > 0)
        {
            int whichAP = Random.Range(0, ap.Count);

            int yeet = Random.Range(0, character.rollAP + 1);

            if (ap.Count == 1)
            {
                switch (ap[whichAP])
                {
                    case "STR":
                        character.str = character.rollAP + 3;
                        break;
                    case "DEX":
                        character.dex = character.rollAP + 3;
                        break;
                    case "INT":
                        character.intel = character.rollAP + 3;
                        break;
                    case "LUK":
                        character.luk = character.rollAP + 3;
                        break;
                }
            }
            else
            {
                switch (ap[whichAP])
                {
                    case "STR":
                        character.str = yeet + 3;
                        break;
                    case "DEX":
                        character.dex = yeet + 3;
                        break;
                    case "INT":
                        character.intel = yeet + 3;
                        break;
                    case "LUK":
                        character.luk = yeet + 3;
                        break;
                }
            }

            ap.RemoveAt(whichAP);
            character.rollAP -= yeet;
        }       
        
        character.rollAP = 7;
        character.rolled = true;
        character.remainingAP = 0;
    }
}
