using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerClass
{
    public Warrior()
    {
        className = "Warrior";
        mainAP = "str";
        secondaryAP = "dex";
        hpModifier = 2.0f;
        mpModifier = 0.75f;
    }
}
