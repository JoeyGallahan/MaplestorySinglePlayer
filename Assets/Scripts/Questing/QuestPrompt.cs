using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class QuestPrompt
{
    public enum QuestType
    {
        KILL = 0, //Need to kill an enemy or multiple enemies
        FETCH = 1, //Need to gather an item or multiple items
        MULTI = 2, //Need to kill enemies AND gather items
        SPEAK = 3  //Need to speak with an NPC
    }

    string[] flags =
    {
        "[ENEMY_ID]",     //0
        "[ITEM_ID]",      //1
        "[NPC_ID]",       //2
        "[AMOUNT_ENEMY]", //3
        "[AMOUNT_ITEM]",  //4
    };

    //The id of the enemy or item you need matched with the amount needed to kill or fetch. 
    //We need an array in case you need to both kill and fetch something
    Dictionary<int, int>[] idToNumber = new Dictionary<int, int>[]
        {
            new Dictionary<int,int>(){ },  //[0] -- for KILL quests <enemy_ids, amtOfEnemies>
            new Dictionary<int, int>(){ }  //[1] -- for FETCH quests <item_ids, amtOfItems>
        };

    [SerializeField]private QuestType promptType;
    public DialogueScene dialogue;

    public void Init(string type)
    {
        switch(type)
        {
            case "KILL": promptType = QuestType.KILL;
                break;
            case "FETCH": promptType = QuestType.FETCH;
                break;
            case "MULTI": promptType = QuestType.MULTI;
                break;
            case "SPEAK": promptType = QuestType.SPEAK;
                break;
            default: promptType = QuestType.SPEAK;
                break;
        }
    }

    public Dictionary<int, int>[] QuestNecessities
    {
        get => idToNumber;
    }
    
    public void LoadQuest()
    {
        foreach (DialogueLine dialogueLine in dialogue.DialogueLines) //Go through each line of Dialogue in this quest
        {
            string fullText = dialogueLine.DialogueText; //Get the actual text string of this line

            if (promptType != QuestType.SPEAK) //If we're doing a speaking quest, don't bother with this stuff
            {
                dialogueLine.DialogueText = LoadQuestValues(dialogueLine.DialogueText); //Replace the dialogue line string with our replacement text

                //If we're doing a quest where we need to both kill enemies AND get items, just run it again and we'll handle it in the function
                if (promptType == QuestType.MULTI)
                {
                    dialogueLine.DialogueText = LoadQuestValues(dialogueLine.DialogueText);
                }
            }
        }
    }

    //Essentially cleans up our dialogue line
    //Removes the flags, gets names of items/enemies, and returns a new, clean string
    //Also adds the items, enemies, and amounts of each we need for this quest to a dictionary array 
    private string LoadQuestValues(string fullText)
    {
        string replacementText = "";

        string idFlag = flags[0]; //start with [ENEMY_ID] by default
        string amtFlag = flags[3]; //start with [AMOUNT_ENEMY] by default
        int dictionaryIndex = 0; //where we're adding to our dictionary [0] = enemies, [1] = items

        //If we're doing a FETCH quest, just switch the flag to [FETCH]
        //If we're doing a MULTI quest, check to see if we've already gotten the enemies to KILL. If we have, switch to get the items we need to FETCH
        if ( promptType == QuestType.FETCH ||
            (promptType == QuestType.MULTI && idToNumber[0].Count > 0)) 
        {
            idFlag = flags[1];
            amtFlag = flags[4];
            dictionaryIndex = 1;
        }

        //Start off by getting the ID of the enemy/item we need to kill/fetch
        string[] newText = fullText.Split(new string[] { idFlag }, System.StringSplitOptions.None); //Split up the text based on the ID flag
        string enemyID = newText[0].Substring(newText[0].LastIndexOf(@" ") + 1);
               
        int id;
        if (int.TryParse(enemyID, out id)) //Converts the string value of the enemyID to an integer
        {
            //Now get the amount of this enemy/item we need to kill/catch
            newText = fullText.Split(new string[] { amtFlag }, System.StringSplitOptions.None); //Split up the text based on the "AMOUNT" flag
            string enemyAmount = newText[0].Substring(newText[0].LastIndexOf(@" ") + 1);

            int amt;
            if (int.TryParse(enemyAmount, out amt)) //Converts the string value of the amount to an integer
            {
                if (dictionaryIndex == 1) //If we're doing item stuff
                {
                    //Remove the Item ID flag, remove the item amount flag, and replace the actual Item ID with the name of the item, and 
                    replacementText += fullText.Replace(idFlag, "").Replace(amtFlag, "").Replace(enemyID, ItemDB.Instance.GetItemName(id));
                    idToNumber[dictionaryIndex].Add(id, amt); //Add a new enemy ID to our list.
                    return replacementText; //Return the new text values
                }

                idToNumber[dictionaryIndex].Add(id, amt); //Add a new enemy ID to our list.
            }
        }

        /*
        Debug.Log("---Debug Dictionary---");
        for (int i = 0; i < 2; i++)
        {
            if (i == 0 )
            {
                Debug.Log("-Enemies-");
            }
            else
            {
                Debug.Log("-Items-");
            }

            foreach (KeyValuePair<int, int> kvp in idToNumber[i])
            {
                Debug.Log("Key = {" + kvp.Key.ToString() + "}, Value = {" + kvp.Value.ToString() + "}");
            }
        }
        */

        return fullText;
    }
}
