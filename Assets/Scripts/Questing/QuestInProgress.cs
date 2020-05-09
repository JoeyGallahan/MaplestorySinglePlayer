using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInProgress
{
    public DialogueScene dialogue;


    public QuestInProgress() { dialogue = new DialogueScene(); }
}
