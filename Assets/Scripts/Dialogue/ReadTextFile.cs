using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadTextFile: MonoBehaviour
{
    char delimiter = '|';
    string[] responseFlags = 
    {
        "[TITLE]",
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

            if (!flag.Equals(responseFlags[0]))
            {
                dialogueLines.Add(new DialogueLine(splitTexts[i], flag));
            }
            else
            {
                scene.Title = splitTexts[i];
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
