using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] int questID;           //the ID of this quest
    [SerializeField] int npcID;             //the npc this quest starts with
    [SerializeField] string questTitle;     //the title of the quest
    [SerializeField] bool questStarted = false;     //If the player has started the quest yet
    [SerializeField] bool questFulfilled = false;   //If the player has killed/gotten all the enemies/items
    [SerializeField] bool questCompleted = false;   //If the player has completely finished the quest and received their reward
    
    public QuestPrompt prompt; //What the quest has you doing
    public QuestReward reward; //The reward for completing the quest

    public int QuestID
    {
        get => questID;
        set
        {
            questID = value;
        }
    }
    public int NPCID
    {
        get => npcID;
        set
        {
            npcID = value;
        }
    }
    public string Title
    {
        get => questTitle;
        set
        {
            questTitle = value;
        }
    }
    public bool QuestStarted
    {
        get => questStarted;
    }
    public bool QuestFulfilled
    {
        get => questFulfilled;
    }
    public bool QuestCompleted
    {
        get => questCompleted;
    }
}
