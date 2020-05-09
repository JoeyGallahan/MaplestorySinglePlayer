using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SkillsGrid : MonoBehaviour
{
    public void AddToGrid(GameObject prefab, Skill skill)
    {
        GameObject newObj;
        newObj = (GameObject)Instantiate(prefab, transform);

        TextMeshProUGUI skillName = newObj.GetComponentInChildren<TextMeshProUGUI>();
        skillName.SetText(skill.skillName);

        Image skillImage = newObj.GetComponentInChildren<Image>();
    }
    public void AddToGrid(GameObject prefab, List<Skill> skills)
    {
        foreach(Skill skill in skills)
        {
            GameObject newObj;
            newObj = (GameObject)Instantiate(prefab, transform);

            TextMeshProUGUI skillName = newObj.GetComponentInChildren<TextMeshProUGUI>();
            skillName.SetText(skill.skillName);

            GameObject sprite = (GameObject)Instantiate(skill.skillSprite, newObj.transform);
            sprite.transform.localScale = new Vector3(0.75f, 0.75f);
        }
    }
}
