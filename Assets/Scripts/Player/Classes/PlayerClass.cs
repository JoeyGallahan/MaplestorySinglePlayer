using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerClass
{
    [SerializeField] protected string className;
    [SerializeField] protected string mainAP;
    protected float hpModifier;
    protected float mpModifier;

    public string ClassName
    {
        get => className;
        set
        {
            className = value;
        }
    }
    public string MainAP
    {
        get => mainAP;
    }
    public float HPModifier
    {
        get => hpModifier;
    }
    public float MPModifier
    {
        get => mpModifier;
    }

}
