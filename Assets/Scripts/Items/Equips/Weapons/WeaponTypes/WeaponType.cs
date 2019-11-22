using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponType
{
    public enum WeaponStyle
    {
        NONE = -1,
        ONEHANDED_SWORD,
        TWOHANDED_SWORD,
        STAFF,
        WAND,
        DAGGER,
        CLAW,
        BOW,
        CROSSBOW
    }
    [SerializeField] WeaponStyle weaponStyle;
    [SerializeField] string typeName;
    [SerializeField] bool melee;
    [SerializeField] string apType;
    [SerializeField] string classCanEquip;

    public string TypeName
    {
        get => typeName;
        set
        {
            typeName = value;
        }
    }
    public bool IsMelee
    {
        get => melee;
        set
        {
            melee = value;
        }
    }
    public string APType
    {
        get => apType;
        set
        {
            apType = value;
        }
    }
    public string ClassName
    {
        get => classCanEquip;
    }
    public WeaponStyle Style { get => weaponStyle; }
}
