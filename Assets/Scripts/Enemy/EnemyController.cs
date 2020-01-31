using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyCharacter enemy;
    PhysicsObject physicsObject;
    EnemyDamageUI damageUI;

    //Movement
    Vector2 patrolLocStart;
    bool patrolling = false;
    bool moveLeft = false;
    float curPatrolDistance = 0.0f;
    bool followingPlayer = false;
    GameObject player;

    //Layers
    int playerLayer = 10;
    int environmentLayer = 9;
    int enemyLayer = 11;
    int itemLayer = 12;

    //Timing
    float patrolWaitTime = 0.0f;
    float maxPatrolWaitTime = 1.0f;
    bool waiting = true;

    private void Awake()
    {
        physicsObject = GetComponent<PhysicsObject>();
        enemy = GetComponent<EnemyCharacter>();        
        damageUI = GetComponent<EnemyDamageUI>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        //Randomize move direction at the start
        int left = Random.Range(0,10);
        if(left <= 4) { moveLeft = true; }
    }

    private void Update()
    {
        if (physicsObject.grounded)
        {
            if (!followingPlayer)
            {
                if (waiting)
                {
                    PatrolCooldown();
                }
                else
                {
                    Patrol();
                }
            }
            else
            {
                FollowPlayer();
            }
        }
    }

    public int TakeDamage(int damage, Vector2 dir)
    {
        Knockback(dir);
        damageUI.AddDamage(damage);

        followingPlayer = true;
        patrolling = false;
        waiting = false;

        int exp = enemy.TakeDamage(damage);

        if (exp > 0)
        {
            physicsObject.SetVelocity(Vector3.zero);
            followingPlayer = false;
        }

        return exp;
    }

    private void Knockback(Vector2 dir)
    {
        dir = new Vector2(dir.x, 3.0f);
        physicsObject.SetVelocity(Vector3.zero);
        physicsObject.grounded = false;
        physicsObject.AddForce(dir * Time.deltaTime);
    }

    private void Patrol()
    {
        if (!patrolling) //If you're not patrolling
        {
            patrolling = true; //Now you are
            patrolLocStart = transform.position; //Get the start location
            moveLeft = !moveLeft; //Make it move the other way
            curPatrolDistance = Random.Range(1.0f, enemy.MaxPatrolDistance);
        }

        if (Vector2.Distance(patrolLocStart, transform.position) >= curPatrolDistance) //If the enemy has reached the end of their patrol range
        {
            patrolling = false; //reset the patrolling process
            waiting = true; //patrol cooldown
            physicsObject.SetVelX(0f); //stop them from moving
        }
        else
        {
            if (moveLeft && !physicsObject.LeftBlocked) //If the enemy is moving left and the left side is not blocked
            {
                physicsObject.SetVelX(-enemy.MoveSpeed * Time.deltaTime); //make them move left
            }
            else if (!moveLeft && !physicsObject.RightBlocked) //If the enemy is moving right and the right side is not blocked
            {
                physicsObject.SetVelX(enemy.MoveSpeed * Time.deltaTime); //make them move right
            }
            else
            {
                patrolling = false; //if you're blocked, reset the patrolling process
                waiting = true; //patrol cooldown
                physicsObject.SetVelX(0f); //stop them from moving
            }
        }
    }

    private void PatrolCooldown()
    {
        if (patrolWaitTime < maxPatrolWaitTime)
        {
            patrolWaitTime += Time.deltaTime;
        }
        else
        {
            patrolWaitTime = 0.0f;
            waiting = false;
        }
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < enemy.MaxFollowDistance)
        {
            if (transform.position.x < player.transform.position.x)
            {
                physicsObject.SetVelX(enemy.MoveSpeed * Time.deltaTime);
            }
            else
            {
                physicsObject.SetVelX(-enemy.MoveSpeed * Time.deltaTime);
            }
        }
        else
        {
            followingPlayer = false;
            waiting = true;
            patrolWaitTime = 0.0f;
        }
    }
}
