using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxMana = 100;
    [SerializeField] private int curHealth = 100;
    [SerializeField] private int curMana = 100;
    private float moveSpeed = 700.0f;
    private float jumpSpeed = 10.0f;
    private float climbSpeed = 500.0f;
    private string playerName = "Player";
    private int baseDamage = 1;

    [SerializeField] private int level = 1;
    [SerializeField] private int experience = 0;
    private int experienceToNextLevel = 100;

    private int strength = 3;
    private int dexterity = 3;
    private int intelligence = 3;
    private int luck = 3;

    [SerializeField] PlayerClass playerClass;
    [SerializeField] PlayerCharacterUI ui;
    bool opened = false;

    public int MaxHealth
    {
        get => maxHealth;

        set
        {
            maxHealth = value;
        }
    }
    public int MaxMana
    {
        get => maxMana;

        set
        {
            maxMana = value;
        }
    }
    public int CurHealth
    {
        get => curHealth;

        set
        {
            curHealth = value;
        }
    }
    public int CurMana
    {
        get => curMana;

        set
        {
            curMana = value;
        }
    }
    public string PlayerName
    {
        get => playerName;

        set
        {
            playerName = value;
        }
    }
    public float MoveSpeed
    {
        get => moveSpeed;

        set
        {
            moveSpeed = value;
        }
    }
    public float JumpSpeed
    {
        get => jumpSpeed;

        set
        {
            jumpSpeed = value;
        }
    }
    public float ClimbSpeed
    {
        get => climbSpeed;

        set
        {
            climbSpeed = value;
        }
    }
    public int Experience
    {
        get => experience;
        set
        {
            experience = value;
        }
    }
    public int ExperienceNeeded
    {
        get => experienceToNextLevel;
        set
        {
            experienceToNextLevel = value;
        }
    }
    public int Level
    {
        get => level;
        set
        {
            level = value;
        }
    }
    public string ClassName
    {
        get => playerClass.ClassName;

    }
    public PlayerCharacterUI UI { get=> ui; }
    public bool Opened { get => ui.Showing(); }
    public int apSTR
    {
        get => strength;

        set
        {
            strength = value;
        }
    }
    public int apDEX
    {
        get => dexterity;

        set
        {
            dexterity = value;
        }
    }
    public int apINT
    {
        get => intelligence;

        set
        {
            intelligence = value;
        }
    }
    public int apLUK
    {
        get => luck;

        set
        {
            luck = value;
        }
    }


    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("CharacterCanvas").GetComponentInChildren<PlayerCharacterUI>();
    }

    public void TakeDamage(int damage)
    {
        curHealth -= damage;
        if (curHealth < 0)
        {
            curHealth = 0;
        }
    }

    private void LateUpdate()
    {
        if (experience >= experienceToNextLevel)
        {
            level++;
            experience = 0;
            experienceToNextLevel *= 2;
        }
    }

    public void LoadCharacterCreation()
    {
        CharacterData temp = CharacterCreationSave.LoadCreatedCharacter();

        //AP
        strength = (int)temp.str;
        dexterity = (int)temp.dex;
        intelligence = (int)temp.intel;
        luck = (int)temp.luk;

        //Info
        playerName = temp.characterName;

        switch (temp.classType)
        {
            case "Warrior": playerClass = new Warrior();
                break;
            case "Bowman": playerClass = new Bowman();
                break;
        }

        maxHealth = Mathf.CeilToInt(playerClass.HPModifier * maxHealth);
        maxMana = Mathf.CeilToInt(playerClass.MPModifier * maxMana);

        curHealth = maxHealth;
        curMana = maxMana;
    }

    private int GetMainAPPoints()
    {
        switch (playerClass.MainAP)
        {
            case "str": return strength;
            case "dex": return dexterity;
            case "int": return intelligence;
            case "luk": return luck;
            default: return strength;
        }
    }

    public int BaseDamage()
    {
        int dmg = baseDamage * GetMainAPPoints();

        return dmg;
    }
}
