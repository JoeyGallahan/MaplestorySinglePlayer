using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterNameUI : MonoBehaviour
{
    public TMP_InputField input;
    [SerializeField] int characterLimit = 15;
    TempCharacter character;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("GameController").GetComponent<TempCharacter>();
        input = GetComponent<TMP_InputField>();
        input.characterLimit = characterLimit;
    }

    private void Update()
    {
        character.characterName = input.text;
    }
}
