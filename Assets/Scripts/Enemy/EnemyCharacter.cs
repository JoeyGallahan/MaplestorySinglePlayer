using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    EnemyInventory inventory;

    [SerializeField] string name;
    [SerializeField] int experience = 100;
    [SerializeField] int health = 100;
    [SerializeField] int level = 1;
    [SerializeField] int touchDamage = 10;

    private void Awake()
    {
        inventory = GetComponent<EnemyInventory>();
    }

    public int TouchDamage
    {
        get => touchDamage;
    }

    private void Update()
    {
    }

    public int TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();

            return experience;
        }

        return 0;
    }

    private void Die()
    {
        inventory.DropItems(transform.position);
        Destroy(this.gameObject);
    }

}
