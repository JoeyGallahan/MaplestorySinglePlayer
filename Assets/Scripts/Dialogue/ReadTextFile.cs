using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadTextFile: MonoBehaviour
{
    char delimiter = '|';
    string[] responseFlags =
    {
        "[NPC_ID]",
        "[TITLE]",
        "[ITEM_ID]",
        "[ENEMY_ID]",
        "[OK]",
        "[NEXT]",
        "[ACCEPT]"
    };

    public void SplitText(string fullText, List<DialogueLine> dialogueLines, DialogueScene scene)
    {
        string[] splitTexts = fullText.Split(delimiter); //split up the text by the delimiter

        for (int i = 0; i < splitTexts.Length; i++) //Go through each one and create a new Dialogue Line
        {
            string flag = GetFlags(splitTexts[i]);
            splitTexts[i] = splitTexts[i].Replace(flag, "");

            if (!flag.Equals(responseFlags[0]) && !flag.Equals(responseFlags[1])) //If the flag is not the title or NPC ID
            {
                dialogueLines.Add(new DialogueLine(splitTexts[i], flag)); //This is a dialogue line
            }
            else if (flag.Equals(responseFlags[0]))
            {
                short id;
                if (Int16.TryParse(splitTexts[i], out id))
                {
                    scene.NPCID = id;
                }
            }
            else if (flag.Equals(responseFlags[1]))
            {
                scene.Title = splitTexts[i].Replace("\n", "");
            }
        }
    }

    private string GetFlags(string text)
    {
        foreach(string flag in responseFlags)
        {
            if (text.Contains(flag))
            {
                return flag;
            }
        }
        return "";
    }
}
