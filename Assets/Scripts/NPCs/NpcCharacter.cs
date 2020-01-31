using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacter : MonoBehaviour
{
    [SerializeField] string npcName;
    [SerializeField] int npcID;

    public string NPCName
    {
        get => npcName;
    }
    public int NPCID
    {
        get => npcID;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
