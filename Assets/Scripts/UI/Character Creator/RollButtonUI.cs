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

        character.str   = 0;
        character.dex   = 0;
        character.intel = 0;
        character.luk   = 0;

        while (ap.Count > 0 && character.rollAP > 0)
        {
            int whichAP = Random.Range(0, ap.Count);

            int yeet = Random.Range(0, character.rollAP + 1);

            if (ap.Count == 1)
            {
                switch (ap[whichAP])
                {
                    case "STR":
                        character.str = character.rollAP;
                        break;
                    case "DEX":
                        character.dex = character.rollAP;
                        break;
                    case "INT":
                        character.intel = character.rollAP;
                        break;
                    case "LUK":
                        character.luk = character.rollAP;
                        break;
                }
            }
            else
            {
                switch (ap[whichAP])
                {
                    case "STR":
                        character.str = yeet;
                        break;
                    case "DEX":
                        character.dex = yeet;
                        break;
                    case "INT":
                        character.intel = yeet;
                        break;
                    case "LUK":
                        character.luk = yeet;
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
