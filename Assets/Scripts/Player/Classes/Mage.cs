﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : PlayerClass
{
    public Mage()
    {
        className = "Mage";
        mainAP = "int";
        secondaryAP = "luk";
        hpModifier = 1.0f;
        mpModifier = 2.0f;
    }

}
