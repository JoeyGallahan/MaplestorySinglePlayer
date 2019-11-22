using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncrementAPUI : MonoBehaviour
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
        if (character.remainingAP > 0)
        {
            char apType = btn.name[0];
            switch (apType)
            {
                case 'S': character.str++;
                    break;
                case 'D': character.dex++;
                    break;
                case 'I': character.intel++;
                    break;
                case 'L': character.luk++;
                    break;
            }
            character.remainingAP--;
        }
    }


}
