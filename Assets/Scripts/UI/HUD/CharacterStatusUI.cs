using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterStatusUI : MonoBehaviour
{
    PlayerCharacter player;

    TextMeshProUGUI hpText;
    TextMeshProUGUI mpText;
    TextMeshProUGUI xpText;

    private void Awake()
    {
        hpText = GameObject.FindGameObjectWithTag("HPText").GetComponent<TextMeshProUGUI>();
        mpText = GameObject.FindGameObjectWithTag("MPText").GetComponent<TextMeshProUGUI>();
        xpText = GameObject.FindGameObjectWithTag("XPText").GetComponent<TextMeshProUGUI>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpText.SetText(player.CurHealth + " / " + player.MaxHealth);
        mpText.SetText(player.CurMana + " / " + player.MaxMana);
        xpText.SetText(player.Experience + " / " + player.ExperienceNeeded);
    }
}
