using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSelect : MonoBehaviour, IPointerClickHandler
{
    SkillsUI ui;

    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("SkillCanvas").GetComponentInChildren<SkillsUI>();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        int id = GetComponentInChildren<SkillID>().skillID;

        ui.UpdateDescription(id);
    }
}