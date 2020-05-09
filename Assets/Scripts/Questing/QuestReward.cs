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
        foreach(KeyValuePair<int,int> kvp in itemReward)
        {
            PlayerInventory.Instance.AddToInventory(kvp.Key, kvp.Value);

            PlayerCharacter.Instance.Experience += expReward;
        }
    }

}
