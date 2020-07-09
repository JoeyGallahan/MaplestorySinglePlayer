using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyDB : MonoBehaviour
{
    private static EnemyDB instance = null;
    private static readonly object padlock = new object();

    public List<EnemyCharacter> enemies = new List<EnemyCharacter>();

    EnemyDB() { }

    public static EnemyDB Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new EnemyDB();
                }
                return instance;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public string GetEnemyName(int enemyID)
    {
        foreach(EnemyCharacter e in enemies)
        {
            if (e.EnemyID == enemyID)
            {
                return e.EnemyName;
            }
        }

        return "uh oh";
    }

}
