using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypeDB : MonoBehaviour
{
    [SerializeField] List<WeaponType> weapons = new List<WeaponType>();

    public WeaponType GetWeaponTypeByName(string weaponName)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].TypeName.Equals(weaponName))
            {
                return weapons[i];
            }
        }

        return null;
    }

    public List<WeaponType> GetWeaponTypesForClass(string className)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].ClassName.Equals(className))
            {
                weapons.Add(weapons[i]);
            }
        }

        return weapons;
    }
}
