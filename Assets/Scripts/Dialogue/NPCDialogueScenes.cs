using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCDialogueScenes : MonoBehaviour
{
    public List<DialogueScene> scenes = new List<DialogueScene>();
    ReadTextFile textReader;

    private void Awake()
    {
        textReader = GameObject.FindGameObjectWithTag("GameController").GetComponent<ReadTextFile>();
    }

    private void Start()
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            textReader.SplitText(scenes[i].TextFile.text, scenes[i].DialogueLines, scenes[i]);
        }
    }
}
