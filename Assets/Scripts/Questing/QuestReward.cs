using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestReward
{
    public DialogueScene dialogue;
    [SerializeField] int expReward;
    [SerializeField] Dictionary<int, int> itemReward = new Dictionary<int, int>();

    public int ExpReward
    {
        get => ExpReward;
        set
        {
            expReward = value;
        }
    }

    public void AddReward(int itemID, int amt)
    {
        if (!itemReward.ContainsKey(itemID))
        {
            itemReward.Add(itemID, amt);
        }
    }

    public void GiveRewards()
    {
        EventParam itemParams = new EventParam();

        foreach(KeyValuePair<int,int> kvp in itemReward)
        {
            itemParams.paramItemID = kvp.Key;
            itemParams.paramInt = kvp.Value;

            EventManager.TriggerEvent("ITEM_PICKUP", itemParams);
        }

        EventParam expParams = new EventParam();
        expParams.paramInt = expReward;
        EventManager.TriggerEvent("EXP_GAIN", expParams);
    }

}
