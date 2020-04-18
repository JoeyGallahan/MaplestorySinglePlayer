using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadTextFile: MonoBehaviour
{
    char delimiter = '|'; //This just tells us that it's at the end of a dialogue line (essentially one page of dialogue)
    string[] textFlags =
    {
        "[NPC_ID]",     //0
        "[TITLE]",      //1
        "[OK]",         //2
        "[NEXT]",       //3
        "[ACCEPT]",     //4
        "[PROMPT]",     //5
        "[REWARD]"      //6
    };

    public void SplitText(string fullText, List<DialogueLine> dialogueLines, DialogueScene scene)
    {
        string[] splitTexts = fullText.Split(delimiter); //split up the text by the delimiter

        for (int i = 0; i < splitTexts.Length; i++) //Go through each one and create a new Dialogue Line
        {
            string flag = GetFlags(splitTexts[i]);
            splitTexts[i] = splitTexts[i].Replace(flag, "");

            if (flag.Equals(textFlags[2]) || flag.Equals(textFlags[3]) || flag.Equals(textFlags[4])) //If the flag is not the title or NPC ID
            {
                dialogueLines.Add(new DialogueLine(splitTexts[i], flag)); //This is a dialogue line
            }
            else if (flag.Equals(textFlags[0])) //NPC ID
            {
                short id;
                if (Int16.TryParse(splitTexts[i], out id))
                {
                    scene.NPCID = id;
                }
            }
            else if (flag.Equals(textFlags[1])) //Title
            {
                scene.Title = splitTexts[i].Replace("\n", "");
            }
        }
    }

    public void SplitText(string fullText, QuestPrompt prompt, Quest quest)
    {
        string[] splitTexts = fullText.Split(delimiter); //split up the text by the delimiter

        for (int i = 0; i < splitTexts.Length; i++) //Go through each one and create a new Dialogue Line
        {
            string flag = GetFlags(splitTexts[i]);
            splitTexts[i] = splitTexts[i].Replace(flag, "");

            if (flag.Equals(textFlags[5])) //If the flag is for a quest prompt
            {
                prompt.Init(splitTexts[i].Replace("\n", "").Replace("\r", ""));

                //Create dialogue lines for your quest
                SplitText(fullText, prompt.dialogue.DialogueLines, prompt.dialogue);
            }
            else if (flag.Equals(textFlags[0])) //for the NPC ID
            {
                short id;
                if (Int16.TryParse(splitTexts[i], out id))
                {
                    quest.NPCID = id;
                }
            }
            else if (flag.Equals(textFlags[1])) //for the title
            {
                quest.Title = splitTexts[i].Replace("\n", "");
            }
        }
    }

    private string GetFlags(string text)
    {
        foreach(string flag in textFlags)
        {
            if (text.Contains(flag))
            {
                return flag;
            }
        }
        return "";
    }
}
