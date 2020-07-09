using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestSelect : MonoBehaviour, IPointerClickHandler
{
    QuestUI ui;
    [SerializeField] GameObject requirementsPrefab;

    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("QuestCanvas").GetComponentInChildren<QuestUI>();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        int id = GetComponentInChildren<QuestID>().ID;
        
        ui.UpdateDescription(id, true);
    }
}