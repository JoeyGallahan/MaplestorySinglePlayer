using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    PlayerCharacter player;
    Image mpBar;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        mpBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        mpBar.fillAmount = (float)player.CurMana / (float)player.MaxMana;
    }
}
