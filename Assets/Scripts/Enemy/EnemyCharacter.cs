using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    EnemyInventory inventory;

    Animator animations;

    [SerializeField] string enemyName;
    [SerializeField] int experience = 100;
    [SerializeField] int health = 100;
    [SerializeField] int level = 1;
    [SerializeField] int touchDamage = 10;
    [SerializeField] float maxPatrolDistance = 5.0f;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float maxFollowDistance = 10.0f;

    bool dead;

    public float MaxPatrolDistance { get => maxPatrolDistance; }
    public float MoveSpeed { get => moveSpeed; }
    public float MaxFollowDistance { get => maxFollowDistance; }

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
            health = 0;
            animations.SetInteger("Health", 0);
            dead = true;
            Die();
        }
    }

    public int TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !dead) //Dont want to return the experience twice (this can/should also be handled when the actual method is called, but this is just a safety check)
        {
            return experience;
        }

        return 0;
    }

    private void Die()
    {
        inventory.DropItems(transform.position);
        Destroy(this.gameObject, animations.GetCurrentAnimatorStateInfo(0).length);
    }

}
