using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityPointUI : MonoBehaviour
{
    TextMeshProUGUI text;
    TempCharacter character;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        character = GameObject.FindGameObjectWithTag("GameController").GetComponent<TempCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText
            (
                "STR: " + character.str +
                "\nDEX: " + character.dex +
                "\nINT: " + character.intel +
                "\nLUK: " + character.luk           
            );
    }
}
