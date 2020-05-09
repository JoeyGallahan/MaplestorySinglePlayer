using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestGrid : MonoBehaviour
{
    public void AddToGrid(GameObject prefab, Quest quest)
    {
        GameObject newObj;
        newObj = (GameObject)Instantiate(prefab, transform);

        TextMeshProUGUI questName = newObj.GetComponentInChildren<TextMeshProUGUI>();
        questName.SetText(quest.Title);

        newObj.GetComponentInChildren<QuestID>().ID = quest.QuestID;
    }
}
