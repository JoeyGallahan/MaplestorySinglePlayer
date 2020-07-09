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

    public void RemoveFromGrid(int questID)
    {
        List<QuestID> quests = new List<QuestID>(GetComponentsInChildren<QuestID>());

        GameObject toRemove = null;
        foreach(QuestID q in quests)
        {
            if (q.ID == questID)
            {
                toRemove = q.gameObject;
            }
        }

        Destroy(toRemove);
    }
}
