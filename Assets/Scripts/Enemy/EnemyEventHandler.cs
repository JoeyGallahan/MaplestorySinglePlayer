using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class EnemyEventHandler : MonoBehaviour
{
    private static EnemyEventHandler instance = null;
    private static readonly object padlock = new object();

    public UnityEvent OnEnemyDeath;

    EnemyEventHandler() { }

    public static EnemyEventHandler Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new EnemyEventHandler();
                }
                return instance;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void EnemyDeathEvent()
    {

    }
}

