using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    EnemyInventory inventory;

    Animator animations;

    [SerializeField] int enemyID;
    [SerializeField] string enemyName;
    [SerializeField] int experience = 100;
    [SerializeField] int health = 100;
    [SerializeField] int level = 1;
    [SerializeField] int touchDamage = 10;
    [SerializeField] float maxPatrolDistance = 5.0f;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float maxFollowDistance = 10.0f;

    bool dead = false;
    [SerializeField] List<int> partOfQuests = new List<int>();
    [SerializeField]EnemySpawn enemySpawn;

    public float MaxPatrolDistance { get => maxPatrolDistance; }
    public float MoveSpeed { get => moveSpeed; }
    public float MaxFollowDistance { get => maxFollowDistance; }
    public int EnemyID { get => enemyID; }
    public string EnemyName { get => enemyName; }

    private void Awake()
    {
        inventory = GetComponent<EnemyInventory>();

        animations = GetComponentInChildren<Animator>();
    }

    public int TouchDamage
    {
        get => touchDamage;
    }

    private void LateUpdate()
    {
        //This is here and not in take damage because of skills that hit twice. 
        //If a skill hits twice, it calls the TakeDamage() twice, which would in turn make it drop double the items and cause a whole bunch of issues.
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Die()
    {
        EventParam deathParams = new EventParam();
        deathParams.paramInt = experience;
        deathParams.paramEnemyID = enemyID;
        EventManager.TriggerEvent("ENEMY_DEATH", deathParams);
        EventManager.TriggerEvent("EXP_GAIN", deathParams);

        animations.SetInteger("Health", 0);
        dead = true;
        enemySpawn = GetComponentInParent<EnemySpawn>();
        enemySpawn.RemoveEnemy(this.gameObject);

        inventory.DropItems(transform.position);
        Destroy(this.gameObject, animations.GetCurrentAnimatorStateInfo(0).length);
    }

    public bool CanDealDamage()
    {
        return !dead;
    }

}
