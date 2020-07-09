using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowman : PlayerClass
{
    public Bowman()
    {
        className = "Bowman";
        mainAP = "dex";
        secondaryAP = "str";
        hpModifier = 0.75f;
        mpModifier = 1.0f;
    }
}