using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyCharacter enemy;
    Rigidbody2D rb;
    EnemyDamageUI damageUI;

    //Layers
    int playerLayer = 10;
    int environmentLayer = 9;
    int enemyLayer = 11;
    int itemLayer = 12;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<EnemyCharacter>();

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer, true);
        Physics2D.IgnoreLayerCollision(itemLayer, enemyLayer, true);

        damageUI = GetComponent<EnemyDamageUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int TakeDamage(int damage, Vector2 dir)
    {
        Knockback(dir);
        damageUI.AddDamage(damage);

        return enemy.TakeDamage(damage);
    }

    private void Knockback(Vector2 dir)
    {
        dir = new Vector2(dir.x, 1.0f);
        rb.AddForce(dir * 1.0f, ForceMode2D.Impulse);
    }
}
