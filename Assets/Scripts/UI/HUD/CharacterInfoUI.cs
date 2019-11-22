using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfoUI : MonoBehaviour
{
    TextMeshProUGUI characterName;
    TextMeshProUGUI characterClass;
    TextMeshProUGUI characterLevel;
    PlayerCharacter player;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();

        characterName = GameObject.FindGameObjectWithTag("CharacterName").GetComponent<TextMeshProUGUI>();
        characterClass = GameObject.FindGameObjectWithTag("CharacterClass").GetComponent<TextMeshProUGUI>();
        characterLevel = GameObject.FindGameObjectWithTag("CharacterLvl").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        characterName.SetText(player.PlayerName);
        characterClass.SetText(player.ClassName);
    }

    private void Update()
    {
        if (!characterLevel.text.ToString().Equals(player.Level.ToString()))
        {
            characterLevel.SetText(player.Level.ToString());
        }
    }

}
