using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadTextFile: MonoBehaviour
{
    char delimiter = '|';
    string[] responseFlags = 
    {
        "[OK]",
        "[NEXT]",
        "[ACCEPT]"
    };

    void Start()
    {
    }

    public void SplitText(string fullText, List<DialogueLine> dialogueLines)
    {
        string[] splitTexts = fullText.Split(delimiter); //split up the text by the delimiter

        for (int i = 0; i < splitTexts.Length; i++) //Go through each one and create a new Dialogue Line
        {
            Debug.Log(splitTexts[i]);
            dialogueLines.Add(new DialogueLine(splitTexts[i], GetFlags(splitTexts[i])));
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
