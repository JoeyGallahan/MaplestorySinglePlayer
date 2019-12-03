using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    PlayerCharacter player;
    Image xpBar;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        xpBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        xpBar.fillAmount = (float)player.Experience / (float)player.ExperienceNeeded;
    }
}
