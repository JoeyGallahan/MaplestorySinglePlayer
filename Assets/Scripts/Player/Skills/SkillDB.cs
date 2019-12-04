using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDB : MonoBehaviour
{
    [SerializeField] List<Skill> skills = new List<Skill>();

    public Skill GetSkillByID(int id)
    {
        foreach (Skill i in skills)
        {
            if (i.id == id)
            {
                return i;
            }
        }

        return null;
    }

    public string GetSkillName(int id)
    {
        foreach (Skill i in skills)
        {
            if (i.id == id)
            {
                return i.skillName;
            }
        }

        return "";
    }

    public GameObject GetSprite(int id)
    {
        foreach (Skill i in skills)
        {
            if (i.id == id)
            {
                return i.skillSprite;
            }
        }

        return null;
    }
}
