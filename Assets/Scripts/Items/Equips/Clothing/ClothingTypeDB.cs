using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingTypeDB : MonoBehaviour
{
    [SerializeField] List<ClothingType> clothes = new List<ClothingType>();

    public ClothingType GetWeaponTypeByName(string weaponName)
    {
        for (int i = 0; i < clothes.Count; i++)
        {
            if (clothes[i].TypeName.Equals(weaponName))
            {
                return clothes[i];
            }
        }

        return null;
    }

    public List<ClothingType> GetClothingTypesForClass(string className)
    {
        for (int i = 0; i < clothes.Count; i++)
        {
            if (clothes[i].ClassName.Equals(className))
            {
                clothes.Add(clothes[i]);
            }
        }

        return clothes;
    }
}