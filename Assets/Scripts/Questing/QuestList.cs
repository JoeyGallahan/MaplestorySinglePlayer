using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestList : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();
    ReadTextFile textReader;

    private void Awake()
    {
        textReader = GetComponent<ReadTextFile>();
    }

    private void Start()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            textReader.SplitText(quests[i].prompt.dialogue.TextFile.text, quests[i].prompt, quests[i]); //Make all the dialogue lines for the quest
            quests[i].prompt.LoadQuest(); //Load the quest details into the prompt
        }
    }

    public List<Quest> GetQuestsByNPCID(int id)
    {
        List<Quest> npcquests = new List<Quest>();

        for (int i = 0; i < quests.Count; i++) //Go through our list of quests
        {
            if (quests[i].NPCID == id) //If the ID matches the one we're looking for
            {
                npcquests.Add(quests[i]); //add it to the list
            }
        }

        return npcquests; //return our list
    }

    public List<DialogueScene> GetQuestDialogueByNPCID(int id)
    {
        List<DialogueScene> npcquests = new List<DialogueScene>();

        for (int i = 0; i < quests.Count; i++) //Go through our list of quests
        {
            if (quests[i].NPCID == id) //If the ID matches the one we're looking for
            {
                if (!quests[i].QuestStarted)
                {
                    npcquests.Add(quests[i].prompt.dialogue);
                }
                else if (quests[i].QuestFulfilled)
                {
                    //npcquests.Add(quests[i].reward.dialogue); //Rewards not set up yet
                }
                else if (quests[i].QuestStarted && !quests[i].QuestFulfilled)
                {
                    //do some sort of dialogue telling the player they're not done yet and remind them what they need to do
                }
            }
        }

        return npcquests; //return our list
    }

    public DialogueScene GetQuestDialogueByID(int id)
    {
        foreach (Quest quest in quests)
        {
            if (quest.QuestID == id)
            {
                if (!quest.QuestStarted)
                {
                    return quest.prompt.dialogue;
                }
                else if (quest.QuestFulfilled)
                {
                    //npcquests.Add(quests[i].reward.dialogue); //Rewards not set up yet
                }
                else if (quest.QuestStarted && !quest.QuestFulfilled)
                {
                    //do some sort of dialogue telling the player they're not done yet and remind them what they need to do
                }
            }
        }

        return null;
    }

    public Quest GetQuestByID(int id)
    {
        foreach (Quest quest in quests)
        {
            if (quest.QuestID == id)
            {
                return quest;
            }
        }

        return null;
    }
}
