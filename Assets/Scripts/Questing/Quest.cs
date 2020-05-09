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
    [SerializeField] int lvlRequired; //The level that is required to start this quest.
    [SerializeField] string description; //A short description of the quest that will show in the UI once you've accepted it.
    
    public QuestPrompt prompt; //What the quest has you doing
    public QuestInProgress inProgress; //What the quest says while you're in progress
    public QuestReward reward; //The reward for completing the quest
    
    //The id of the enemy or item you need matched with the amount needed to kill or fetch. 
    //We need an array in case you need to both kill and fetch something
    Dictionary<int, int>[] questRequirements = new Dictionary<int, int>[]
        {
            new Dictionary<int,int>(){ },  //[0] -- for KILL quests <enemy_ids, amtOfEnemies>
            new Dictionary<int, int>(){ }  //[1] -- for FETCH quests <item_ids, amtOfItems>
        };

    public Quest(int id, string title)
    {
        questID = id;
        questTitle = title;
    }

    public enum TypeOfQuest
    {
        KILL  = 0, //Need to kill an enemy or multiple enemies
        FETCH = 1, //Need to gather an item or multiple items
        MULTI = 2, //Need to kill enemies AND gather items
        SPEAK = 3  //Need to speak with an NPC
    }
    [SerializeField] private TypeOfQuest questType;

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
        set
        {
            questFulfilled = value;
        }
    }
    public bool QuestCompleted
    {
        get => questCompleted;
    }
    public TypeOfQuest QuestType
    {
        get => questType;
        set
        {
            questType = value;
        }
    }
    public Dictionary<int, int> ItemsRequired
    {
        get => questRequirements[1];
    }
    public Dictionary<int, int> EnemiesRequired
    {
        get => questRequirements[0];
    }
    public Dictionary<int, int>[] QuestRequirements
    {
        get => questRequirements;
    }
    public int LevelRequired
    {
        get => lvlRequired;
        set
        {
            lvlRequired = value;
        }
    }
    public string Description
    {
        get => description;
        set
        {
            description = value;
        }
    }

    public DialogueScene GetCurrentDialogueScene()
    {
        if (!questStarted)
        {
            return prompt.dialogue;
        }
        else if (!questFulfilled)
        {
            return inProgress.dialogue;
        }
        else if (questFulfilled)
        {
            return reward.dialogue;
        }

        return null;
    }

    public void StartQuest()
    {
        questStarted = true;
    }
    public void CompleteQuest()
    {
        questCompleted = true;
        reward.GiveRewards();
    }

    public void AddToRequirements(int index, int id, int amt)
    {
        questRequirements[index].Add(id, amt);
    }

    public void DebugDictionary()
    {
        Debug.Log("---Debug Dictionary---");
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                Debug.Log("-Enemies-");
            }
            else
            {
                Debug.Log("-Items-");
            }

            foreach (KeyValuePair<int, int> kvp in questRequirements[i])
            {
                Debug.Log("Key = {" + kvp.Key.ToString() + "}, Value = {" + kvp.Value.ToString() + "}");
            }
        }
    }

    public void AddToRewards(int item, int amt)
    {
        reward.AddReward(item, amt);
    }

    public void SetExpReward(int exp)
    {
        reward.ExpReward = exp;
    }
}
