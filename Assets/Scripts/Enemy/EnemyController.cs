using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyCharacter enemy;
    PhysicsObject physicsObject;
    EnemyDamageUI damageUI;

    //Layers
    int playerLayer = 10;
    int environmentLayer = 9;
    int enemyLayer = 11;
    int itemLayer = 12;

    private void Awake()
    {
        physicsObject = GetComponent<PhysicsObject>();
        enemy = GetComponent<EnemyCharacter>();        
        damageUI = GetComponent<EnemyDamageUI>();
    }

    private void Update()
    {
        if (physicsObject.grounded)
        {
            physicsObject.SetVelX(0f);
        }
    }

    public int TakeDamage(int damage, Vector2 dir)
    {
        Knockback(dir);
        damageUI.AddDamage(damage);

        return enemy.TakeDamage(damage);
    }

    private void Knockback(Vector2 dir)
    {
        dir = new Vector2(dir.x, 3.0f);
        physicsObject.SetVelY(0f);
        physicsObject.SetVelX(0f);
        physicsObject.grounded = false;
        physicsObject.AddForce(dir * Time.deltaTime);
    }
}
