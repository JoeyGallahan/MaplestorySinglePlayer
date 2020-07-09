using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

public class QuestLoader
{
    QuestFlags questFlags;
    ItemDB itemDB;

    public void LoadQuests(TextAsset questFile, TextAsset questFlagsFile)
    {
        questFlags.ParseFlagFile(questFlagsFile.text);
        ParseQuestFile(questFile.text);
    }

    void ParseQuestFile(string xmlData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData);

        //Go through every quest
        int questIndex = 0;
        foreach(XmlNode node in xmlDoc["Quests"].ChildNodes)
        {
            int id = StringToInt(node.Attributes["id"].InnerText); //grab the id of the quest
            string title = node.Attributes["name"].InnerText; //grab the title of the quest

            QuestDB.Instance.AddQuest(new Quest(id, title)); //create a new quest

            //Go through every aspect of the quest
            foreach (XmlNode questXML in node.ChildNodes)
            {
                switch (questXML.Name)
                {
                    case "Prompt": LoadQuestPrompt(QuestDB.Instance.GetQuestByID(id), questXML);
                        break;
                    case "Progress": LoadQuestProgress(QuestDB.Instance.GetQuestByID(id), questXML);
                        break;
                    case "Reward": LoadQuestReward(QuestDB.Instance.GetQuestByID(id), questXML);
                        break;
                    default: LoadBasicQuestInfo(QuestDB.Instance.GetQuestByID(id), questXML);
                        break;
                }
            }
            questIndex++;
        }
    }

    //Loads general information like the id of the NPC that starts the quest and the type of quest it is 
    void LoadBasicQuestInfo(Quest quest, XmlNode node)
    {
        string attribute = node.Name;
        switch (attribute)
        {
            case "NPC_ID":
                int npcID = StringToInt(node.InnerText);
                if (npcID != -1)
                {
                    quest.NPCID = npcID;
                }
                break;
            case "Type": quest.QuestType = (Quest.TypeOfQuest)questFlags.questTypeFlags[node.InnerText];
                break;
            case "Description": quest.Description = node.InnerText;
                break;
            case "Level":
                int levelReq = StringToInt(node.InnerText);
                if (levelReq != -1)
                {
                    quest.LevelRequired = levelReq;
                }
                break;
        }
    }

    //Loads the information for the quest prompt
    void LoadQuestPrompt(Quest quest, XmlNode node)
    {
        quest.prompt = new QuestPrompt();

        int itemID = -1, enemyID = -1;
        int itemAmt = -1, enemyAmt = -1;

        foreach (XmlNode child in node.ChildNodes)
        {
            string attribute = child.Name;
            //Debug.Log("Prompt: " + attribute);

            switch(attribute)
            {
                case "Item_ID": 
                    int i_id = StringToInt(child.InnerText);
                    itemID = i_id;
                    break;
                case "Item_Amount":
                    int i_amt = StringToInt(child.InnerText);
                    itemAmt = i_amt;
                    break;
                case "Dialogue": quest.prompt.dialogue = new DialogueScene();
                    LoadDialogue(quest.prompt.dialogue, child, quest);
                    break;
                case "Enemy_ID":
                    int e_id = StringToInt(child.InnerText);
                    enemyID = e_id;
                    break;
                case "Enemy_Amount":
                    int e_amt = StringToInt(child.InnerText);
                    enemyAmt = e_amt;
                    break;
            }
            if (enemyID > -1 && enemyAmt > -1)
            {
                //Debug.Log("Enemy ID: " + enemyID + " Enemy: " + enemyAmt);
                quest.AddToRequirements(0, enemyID, enemyAmt);
                enemyID = -1;
                enemyAmt = -1;
            }
            if (itemID > -1 && itemAmt > -1)
            {
                //Debug.Log("Item ID: " + itemID + " Amt: " + itemAmt);
                quest.AddToRequirements(1, itemID, itemAmt);
                itemID = -1;
                itemAmt = -1;
            }

        }

        //quest.DebugDictionary();
    }

    //Loads the information for when a quest is still in progress
    void LoadQuestProgress(Quest quest, XmlNode node)
    {
        quest.inProgress = new QuestInProgress();

        foreach (XmlNode child in node.ChildNodes)
        {
            string attribute = child.Name;
            //Debug.Log("Progress: " + attribute + " " + child.InnerText);
            if (attribute.Equals("Dialogue"))
            {
                LoadDialogue(quest.inProgress.dialogue, child, quest);
            }
        }
    }

    //Loads the information for the quest's reward
    void LoadQuestReward(Quest quest, XmlNode node)
    {
        quest.reward = new QuestReward();

        int itemID = -1, itemAmt = -1;

        foreach (XmlNode child in node.ChildNodes)
        {
            string attribute = child.Name;
            //Debug.Log("Reward: " + attribute);

            switch (attribute)
            {
                case "EXP":
                    int exp = StringToInt(child.InnerText);
                    quest.SetExpReward(exp);
                    break;
                case "Item_ID":
                    int i_id = StringToInt(child.InnerText);
                    itemID = i_id;
                    break;
                case "Item_Amount":
                    int i_amt = StringToInt(child.InnerText);
                    itemAmt = i_amt;
                    break;
                case "Dialogue":
                    quest.reward.dialogue = new DialogueScene();
                    LoadDialogue(quest.reward.dialogue, child, quest);
                    break;
            }
            if (itemID > -1 && itemAmt > -1)
            {
                quest.AddToRewards(itemID, itemAmt);
                itemID = -1;
                itemAmt = -1;
            }

        }
    }

    void LoadDialogue(DialogueScene scene, XmlNode node, Quest quest)
    {
        foreach (XmlNode child in node.ChildNodes)
        {
            string attribute = child.Name;
            //Debug.Log("Dialogue Node: " + attribute + " " + child.InnerText);
            if (attribute.Equals("Line"))
            {
                string lineText = "";
                foreach (XmlNode response in child.ChildNodes)
                {
                    if (response.Name.Equals("Response"))
                    {
                        //Debug.Log("Adding line: " + lineText + " flag " + response.InnerText);
                        scene.AddDialogueLine(new DialogueLine(lineText, questFlags.questResponseFlags[response.InnerText]));
                        lineText = "";
                    }
                    else
                    {
                        lineText = response.InnerText;
                    }
                }
            }
        }
    }

    //Changes a string to an int if possible
    int StringToInt(string text)
    {
        int ret = -1;
        int.TryParse(text, out ret);
        return ret;
    }

    //Contains and handles all flags for a quest
    struct QuestFlags
    {
        public Dictionary<string, int> questTypeFlags, questResponseFlags, dialogueFlags;

        private void InitVars()
        {
            questTypeFlags = new Dictionary<string, int>();
            questResponseFlags = new Dictionary<string, int>();
            dialogueFlags = new Dictionary<string, int>();
        }

        public void ParseFlagFile(string xmlData)
        {
            InitVars();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);

            foreach (XmlNode node in xmlDoc["Quest_Flags"].ChildNodes)
            {
                Dictionary<string, int> tempDict = new Dictionary<string, int>();

                foreach (XmlNode questXML in node.ChildNodes)
                {
                    tempDict.Add(questXML.Name, StringToInt(questXML.InnerText));
                }

                switch(node.Name)
                {
                    case "Types": questTypeFlags = tempDict;
                        break;
                    case "Responses": questResponseFlags = tempDict;
                        break;
                    case "Dialogue": dialogueFlags = tempDict;
                        break;
                }
            }
        }

        int StringToInt(string text)
        {
            int ret = -1;
            int.TryParse(text, out ret);
            return ret;
        }
    }

}
