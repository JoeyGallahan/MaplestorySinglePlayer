using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class QuestDB : MonoBehaviour
{
    [SerializeField] TextAsset questFile;
    [SerializeField] TextAsset questFlagsFile;

    public List<Quest> quests = new List<Quest>();
    QuestLoader questLoader = new QuestLoader();

    private static QuestDB instance = null;
    private static readonly object padlock = new object();

    QuestDB() { }

    public static QuestDB Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new QuestDB();
                }
                return instance;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        questLoader.LoadQuests(questFile, questFlagsFile);
    }

    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
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

    public List<DialogueScene> GetDialoguesByNPCID(int id)
    {
        List<DialogueScene> scenes = new List<DialogueScene>();

        for (int i = 0; i < quests.Count; i++) //Go through our list of quests
        {
            if (quests[i].NPCID == id) //If the ID matches the one we're looking for
            {
                if (!quests[i].QuestStarted)
                {
                    scenes.Add(quests[i].prompt.dialogue);
                }
                else if (!quests[i].QuestFulfilled)
                {
                    scenes.Add(quests[i].inProgress.dialogue);
                }
                else if (quests[i].QuestFulfilled)
                {
                    //scenes.Add(quests[i].reward.dialogue);
                }
            }
        }

        return scenes;
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
