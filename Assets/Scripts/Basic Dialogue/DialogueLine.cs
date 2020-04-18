using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [SerializeField] string dialogueText;
    [SerializeField] DialogueResponse response;

    public DialogueResponse Response
    {
        get => response;
    }

    public DialogueLine(string text, string flag)
    {
        dialogueText = text;
        
        switch(flag)
        {
            case "[OK]": response = new DialogueResponse(DialogueResponse.ResponseType.END);
                break;
            case "[NEXT]": response = new DialogueResponse(DialogueResponse.ResponseType.CONTINUE);
                break;
            case "[ACCEPT]": response = new DialogueResponse(DialogueResponse.ResponseType.ACCEPT);
                break;
        }
    }

    public string DialogueText
    {
        get => dialogueText;
        set
        {
            dialogueText = value;
        }
    }
}