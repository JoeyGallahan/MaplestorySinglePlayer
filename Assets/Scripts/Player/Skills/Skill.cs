using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Skill : ScriptableObject
{
    protected PlayerCharacter player; //The actual player. This is used for things like finding enemies/party members in range.
    protected LayerMask enemyLayer;
    protected UIController uiController;

    [SerializeField] public int id = -1; //The id of the skill
    [SerializeField] public string skillName; //The name of the skill
    [SerializeField] public string classRequired; //The class a player must be to use this skill 
    [SerializeField] protected WeaponType.WeaponStyle[] requiredWeapons; //The weapons that need to be equipped to use this skill
    [SerializeField] public int levelRequired; //The level required to use this skill
    [SerializeField] public int mpUsed;
    [SerializeField] public string description; //The description that will show on the skill UI
    [SerializeField] public GameObject skillSprite; //The sprite that will show on the UI
    [SerializeField] protected float range; //The range of the skill
    [SerializeField] protected Animation skillAnimation; //The animation the player performs when using this skill

    [SerializeField] protected GameObject playerSkillPrefab; //What will show on the player when the skill is used, if anything
    [SerializeField] protected GameObject enemySkillPrefab; //What will show on the enemy when the skill is used, if anything

    [SerializeField] protected GameObject[] targets; //The targets the skill will be used on

    public abstract void UseSkill(); //Do the thing
    protected abstract void GetTargets(); //Get the targets for the skill

    protected virtual void UpdatePlayerData()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        uiController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }
    protected virtual void PlayEnemySkill()
    {
        foreach(GameObject t in targets)
        {
            if (t != null)
            {
                GameObject prefab = Instantiate(enemySkillPrefab, t.transform);
                Destroy(prefab, 1);
            }
        }
    }
    protected virtual bool UseMP()
    {
        if (player.CurMana < mpUsed)
        {
            return false;
        }
        else
        {
            player.CurMana -= mpUsed;

            return true;
        }
    }
}
