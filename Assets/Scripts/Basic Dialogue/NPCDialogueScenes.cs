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
        textReader = GetComponent<ReadTextFile>();
    }

    private void Start()
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            scenes[i].SceneID = i + 6;
            textReader.SplitText(scenes[i].TextFile.text, scenes[i].DialogueLines, scenes[i]);
        }
    }

    public List<DialogueScene> GetScenesByNPCID(int id)
    {
        List<DialogueScene> npcScenes = new List<DialogueScene>();

        for (int i = 0; i < scenes.Count; i++) //Go through our list of scenes
        {
            if (scenes[i].NPCID == id) //If the ID matches the one we're looking for
            {
                npcScenes.Add(scenes[i]); //add it to the list
            }
        }

        return npcScenes; //return our list
    }

    public DialogueScene GetSceneByID(int id)
    {
        foreach (DialogueScene scene in scenes)
        {
            if (scene.SceneID == id)
            {
                return scene;
            }
        }

        return null;
    }
}
