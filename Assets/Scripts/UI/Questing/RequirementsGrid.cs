using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequirementsGrid : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    public void NewGrid(Quest quest)
    {
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }

        List<int> itemKeysUsed = new List<int>();
        List<int> enemyKeysUsed = new List<int>();

        foreach (KeyValuePair<int, int> kvp in quest.ItemsRequired)
        {
            if (!itemKeysUsed.Contains(kvp.Key))
            {
                itemKeysUsed.Add(kvp.Key);

                GameObject newObj;
                newObj = (GameObject)Instantiate(prefab, transform);

                //Figure out a not shit way to do this for the love of god
                List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>(newObj.GetComponentsInChildren<TextMeshProUGUI>());
                foreach (TextMeshProUGUI t in texts)
                {
                    //Debug.Log("NAME: " + t.name);
                    switch (t.name)
                    {
                        case "Item/Enemy":
                            t.text = ItemDB.Instance.GetItemName(kvp.Key);
                            break;
                        case "Need Amount":
                            t.text = kvp.Value.ToString();
                            break;
                        case "Have Amount":
                            t.text = PlayerCharacter.Instance.GetActiveQuestByID(quest.QuestID).ItemAmounts[kvp.Key].ToString();
                            break;
                    }
                }
            }
        }
        foreach (KeyValuePair<int, int> kvp in quest.EnemiesRequired)
        {
            if(!enemyKeysUsed.Contains(kvp.Key))
            {
                enemyKeysUsed.Add(kvp.Key);

                GameObject newObj;
                newObj = (GameObject)Instantiate(prefab, transform);

                //Figure out a not shit way to do this for the love of god
                List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>(newObj.GetComponentsInChildren<TextMeshProUGUI>());
                foreach (TextMeshProUGUI t in texts)
                {
                    //Debug.Log("NAME: " + t.name);
                    switch (t.name)
                    {
                        case "Item/Enemy":
                            t.text = EnemyDB.Instance.GetEnemyName(kvp.Key);
                            break;
                        case "Need Amount":
                            t.text = kvp.Value.ToString();
                            break;
                        case "Have Amount":
                            t.text = PlayerCharacter.Instance.GetActiveQuestByID(quest.QuestID).EnemyAmounts[kvp.Key].ToString();
                            break;
                    }
                }
            }
        }
    }

    public void UpdateCurrentGrid(Quest quest)
    {
        foreach (KeyValuePair<int, int> kvp in quest.ItemsRequired)
        {
            //Figure out a not shit way to do this for the love of god
            List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>());
            foreach (TextMeshProUGUI t in texts)
            {
                //Debug.Log("NAME: " + t.name);
                if (t.name.Equals("Have Amount"))
                {
                    t.text = PlayerCharacter.Instance.GetActiveQuestByID(quest.QuestID).ItemAmounts[kvp.Key].ToString();
                }
            }
        }
        foreach (KeyValuePair<int, int> kvp in quest.EnemiesRequired)
        {
            //Figure out a not shit way to do this for the love of god
            List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>());
            foreach (TextMeshProUGUI t in texts)
            {
                //Debug.Log("NAME: " + t.name);
                if (t.name.Equals("Have Amount"))
                { 
                    t.text = PlayerCharacter.Instance.GetActiveQuestByID(quest.QuestID).EnemyAmounts[kvp.Key].ToString();
                }
            }
        }
    }
}
