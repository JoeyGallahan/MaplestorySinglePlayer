using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class NPCDB : MonoBehaviour
{
    [SerializeField] List<NpcCharacter> npcs = new List<NpcCharacter>();

    private static NPCDB instance = null;
    private static readonly object padlock = new object();

    NPCDB() { }

    public static NPCDB Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new NPCDB();
                }
                return instance;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public string GetNpcName(int npcID)
    {
        foreach(NpcCharacter npc in npcs)
        {
            if (npc.NPCID == npcID)
            {
                return npc.NPCName;
            }
        }

        return "NPC";
    }

}
