﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActiveQuest
{
    [SerializeField]public Quest quest;

    //The id of the enemy or item you need matched with the amount you have killed or gathered. 
    //We need an array in case you need to both kill and fetch something
    Dictionary<int, int>[] idToAmount = new Dictionary<int, int>[]
        {
            new Dictionary<int,int>(){ },  //[0] -- for KILL quests <enemy_ids, amtOfEnemiesKilled>
            new Dictionary<int, int>(){ }  //[1] -- for FETCH quests <item_ids, amtOfItemsGathered>
        };

    public Dictionary<int, int> ItemAmounts
    {
        get => idToAmount[1];
    }
    public Dictionary<int, int> EnemyAmounts
    {
        get => idToAmount[0];
    }
    public ActiveQuest(int questID)
    {
        Init(questID);
    }

    public void Init(int questID)
    {
        quest = QuestDB.Instance.GetQuestByID(questID);

        foreach (KeyValuePair<int, int> kvp in quest.EnemiesRequired)
        {
            idToAmount[0].Add(kvp.Key, 0);
        }
        foreach (KeyValuePair<int, int> kvp in quest.ItemsRequired)
        {
            if (PlayerInventory.Instance.ContainsID(kvp.Key))
            {                
                idToAmount[1].Add(kvp.Key, PlayerInventory.Instance.GetAmountByID(kvp.Key));
                if (idToAmount[1][kvp.Key] >= quest.ItemsRequired[kvp.Key])
                {
                    Debug.Log("Quest Completed wow so fast!");
                    quest.QuestFulfilled = true;
                }
            }
            else
            {
                idToAmount[1].Add(kvp.Key, 0);
            }
        }
    }

    public void AddToEnemyProgress(int enemyID, int amt)
    {
        idToAmount[0][enemyID] += amt;
    }
    public void AddToItemProgress(int itemID, int amt)
    {
        idToAmount[1][itemID] += amt;

        if (idToAmount[1][itemID] >= quest.ItemsRequired[itemID])
        {
            Debug.Log("Quest Completed!");
            quest.QuestFulfilled = true;
        }
    }
}
