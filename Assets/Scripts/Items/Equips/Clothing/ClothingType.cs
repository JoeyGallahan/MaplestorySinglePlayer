using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClothingType
{
    public enum ClothingStyle
    {
        NONE = -1,
        HEAD = 0,
        TOP = 1,
        BOTTOM = 2,
        FEET = 3,
        GLOVES = 4,
        RING = 5,
        CAPE = 6
    }

    [SerializeField] ClothingStyle clothingStyle;
    [SerializeField] string typeName;
    [SerializeField] string classCanEquip;

    public string TypeName
    {
        get => typeName;
        set
        {
            typeName = value;
        }
    }
    public string ClassName
    {
        get => classCanEquip;
    }
    public ClothingStyle Style { get => clothingStyle; }

    public GameObject GetEquipUISlot()
    {
        switch(clothingStyle)
        {
            case ClothingStyle.TOP: return GameObject.FindGameObjectWithTag("EquipBody");
            case ClothingStyle.HEAD: return GameObject.FindGameObjectWithTag("EquipHead");
            case ClothingStyle.BOTTOM: return GameObject.FindGameObjectWithTag("EquipBottom");
            default: return null;
        }
    }

    public GameObject GetEquipmentSlot()
    {
        switch (clothingStyle)
        {
            case ClothingStyle.TOP: return GameObject.FindGameObjectWithTag("Top");
            case ClothingStyle.HEAD: return GameObject.FindGameObjectWithTag("Head");
            case ClothingStyle.BOTTOM: return GameObject.FindGameObjectWithTag("Bottom");
            default: return null;
        }
    }
}
