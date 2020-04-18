using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSceneID : MonoBehaviour
{
    [SerializeField] int sceneID;
    public int SceneID
    {
        get => sceneID;
        set
        {
            sceneID = value;
        }
    }
}
