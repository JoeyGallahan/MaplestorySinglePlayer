using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecrementAPUI : MonoBehaviour
{
    Button btn;
    TempCharacter character;

    private void Start()
    {
        character = GameObject.FindGameObjectWithTag("GameController").GetComponent<TempCharacter>();

        btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        char apType = btn.name[0];
        switch (apType)
        {
            case 'S':
                if (!character.rolled && character.str > 3)
                {
                    character.str--;
                    character.remainingAP++;
                }
                break;
            case 'D':
                if (!character.rolled && character.dex > 3)
                {
                    character.dex--;
                    character.remainingAP++;
                }
                break;
            case 'I':
                if (!character.rolled && character.intel > 3)
                {
                    character.intel--;
                    character.remainingAP++;
                }
                break;
            case 'L':
                if (!character.rolled && character.luk > 3)
                {
                    character.luk--;
                    character.remainingAP++;
                }
                break;
        }
    }


}
