using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButtonUI : MonoBehaviour
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
        character.remainingAP = 5;
        character.str = 3.0f;
        character.dex = 3.0f;
        character.intel = 3.0f;
        character.luk = 3.0f;

        character.rolled = false;
    }

}
