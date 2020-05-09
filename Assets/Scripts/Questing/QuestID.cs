using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestID : MonoBehaviour
{
    [SerializeField] int questID;

    public int ID
    {
        get => questID;
        set
        {
            questID = value;
        }
    }
}
