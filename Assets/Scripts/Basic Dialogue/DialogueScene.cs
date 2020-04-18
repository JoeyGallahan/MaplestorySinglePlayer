using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueScene
{
    [SerializeField] int npcID;
    [SerializeField] int sceneID;
    [SerializeField] string sceneTitle;
    [SerializeField] int requiredLevel;
    [SerializeField] TextAsset textFile;
    [SerializeField] List<DialogueLine> dialogueLines = new List<DialogueLine>();
    
    public int SceneID
    {
        get => sceneID;
        set
        {
            sceneID = value;
        }
    }
    public string Title
    {
        get => sceneTitle;
        set
        {
            sceneTitle = value;
        }
    }
    public int RequiredLevel
    {
        get => requiredLevel;
    }
    public int NPCID
    {
        get => npcID;
        set
        {
            npcID = value;
        }
    }
    public TextAsset TextFile
    {
        get => textFile;
    }
    public List<DialogueLine> DialogueLines
    {
        get => dialogueLines;
    }

    public DialogueLine GetLine(int index)
    {
        return dialogueLines[index];
    }
}
