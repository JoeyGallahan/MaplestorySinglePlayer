using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerCharacter : MonoBehaviour
{
    //Statuses
    private int maxHealth = 100;
    private int maxMana = 100;
    private int curHealth = 100;
    private int curMana = 100;

    //Movement
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 20.0f;
    [SerializeField] private float climbSpeed = 5.0f;

    //Player specifics
    [SerializeField]private string playerName = "Player";
    [SerializeField] PlayerClass playerClass;
    EquippedItems equips;
    ParticleSystem levelUpParticles;

    //Attacking
    private int baseDamage = 3;
    private int maxDamage = 3;
    [SerializeField]private float baseAttackRange = 0.25f;
    private float baseAttackSpeed = 1.0f;

    //Levels
    [SerializeField] private int level = 1;
    [SerializeField] private int experience = 0;
    private int experienceToNextLevel = 100;

    PlayerCharacterUI ui;

    //AP
    private int remainingAPPoints = 0;
    private int originalRemaining = 0;
    private Dictionary<string, int> originalAP;
    private Dictionary<string, int> apPoints;
    
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
    public int apSTR
    {
        get => apPoints["str"];

        set
        {
            apPoints["str"] = value;
        }
    }
    public int apDEX
    {
        get => apPoints["dex"];

        set
        {
            apPoints["dex"] = value;
        }
    }
    public int apINT
    {
        get => apPoints["int"];

        set
        {
            apPoints["int"] = value;
        }
    }
    public int apLUK
    {
        get => apPoints["luk"];

        set
        {
            apPoints["luk"] = value;
        }
    }
    public int RemainingApPoints
    {
        get => remainingAPPoints;
        set
        {
            remainingAPPoints = value;
        }
    }
    public int MaxDamage
    {
        get => maxDamage;
    }
    public int BaseDamage { get => baseDamage; }
    public float BaseAttackRange { get => baseAttackRange; }
    public float BaseAttackSpeed { get => baseAttackSpeed; }
    public EquippedItems Equips { get => equips; }


    private void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("CharacterCanvas").GetComponentInChildren<PlayerCharacterUI>();
        originalAP = new Dictionary<string, int>();
        equips = GetComponent<EquippedItems>();

        levelUpParticles = GetComponentInChildren<ParticleSystem>();
        LoadCharacterCreation();
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
            LevelUp();
        }
    }

    public void LoadCharacterCreation()
    {
        CharacterData temp = CharacterCreationSave.LoadCreatedCharacter();

        //Load in the AP
        apPoints = new Dictionary<string, int>
        {
            { "str", temp.str },
            { "dex", temp.dex },
            { "int", temp.intel },
            { "luk", temp.luk }
        };

        //Info
        playerName = temp.characterName;
        switch (temp.classType)
        {
            case "Warrior": playerClass = new Warrior();
                break;
            case "Bowman": playerClass = new Bowman();
                break;
        }

        //Statuses
        maxHealth = Mathf.CeilToInt(playerClass.HPModifier * maxHealth);
        maxMana = Mathf.CeilToInt(playerClass.MPModifier * maxMana);
        curHealth = maxHealth;
        curMana = maxMana;

        UpdateDamageRange();
    }

    public int GetMainAPPoints()
    {
        return apPoints[playerClass.MainAP];
    }

    public int GetDamage()
    {
        int dmg = Mathf.RoundToInt(Random.Range(baseDamage, maxDamage + 1));

        return dmg;
    }

    public void LevelUp()
    {
        level++;
        experience = 0;
        experienceToNextLevel *= 2;
        remainingAPPoints += 5;
        UpdateDamageRange();
        ui.UpdateTexts();
        ui.ToggleAPChanges(true);
        levelUpParticles.Play();
    }

    private void UpdateDamageRange()
    {
        baseDamage = Mathf.RoundToInt(GetMainAPPoints() * level / 5) + equips.GetEquipDamage();
        maxDamage = baseDamage * 2;
    }

    public void IncAP(string apType)
    {
        if (remainingAPPoints > 0)
        {
            if (originalAP.Count == 0)
            {
                originalAP = new Dictionary<string, int>(apPoints);
                originalRemaining = remainingAPPoints;
            }

            apPoints[apType]++;
            remainingAPPoints--;
            UpdateDamageRange();
        }
        ui.UpdateTexts();
    }

    public void DecAP(string apType)
    {
        if (originalAP.Count == 0)
        {
            originalAP = new Dictionary<string, int>(apPoints);
            originalRemaining = remainingAPPoints;
        }

        if (apPoints[apType] > originalAP[apType])
        {
            remainingAPPoints++;
            apPoints[apType]--;
            UpdateDamageRange();
        }
        ui.UpdateTexts();
    }

    public void SaveAP()
    {
        originalAP = new Dictionary<string, int>();
        ui.UpdateTexts();
    }

    public void CancelAP()
    {
        apPoints = new Dictionary<string, int>(originalAP);
        remainingAPPoints = originalRemaining;

        UpdateDamageRange();
        ui.UpdateTexts();
    }

    public void UpdateEquip(string type, int id)
    {
        equips.UpdateEquip(type, id);
        UpdateDamageRange();
        ui.AddEquip(type, id);
    }
}
