using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDB : MonoBehaviour
{
    [SerializeField] List<Skill> skills = new List<Skill>();

    public List<Skill> GetSkillsByClass(string classType)
    {
        List<Skill> classSkills = new List<Skill>();
        foreach (Skill i in skills)
        {
            if (i.classRequired.Equals(classType)) 
            {
                classSkills.Add(i);
            }
        }

        return classSkills;
    }

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
