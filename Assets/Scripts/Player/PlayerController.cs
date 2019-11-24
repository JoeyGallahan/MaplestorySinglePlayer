using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Other instances we need to control the player
    PlayerCharacter playerCharacter;
    Animator animations;
    Rigidbody2D rb;

    //Player states
    [SerializeField] bool grounded = true;
    [SerializeField] bool climbing = false;
    [SerializeField] bool touchingRope = false;

    //Movement and rotations
    Vector3 movement = Vector3.zero;
    float horInput = 0.0f;
    Quaternion facingLeft = Quaternion.Euler(Vector3.zero);
    Quaternion facingRight = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    //Layers
    int playerLayer;
    int environmentLayer;
    int itemLayer = 12;
    LayerMask enemyLayer;
    
    //Attacks
    [SerializeField] bool canAttack = true;
    float attackTime;

    //Damage
    [SerializeField] float damageTime;
    [SerializeField] bool damaged = false;

    float defaultGravity;

    //Inventory
    [SerializeField] GameObject itemTouching;
    ItemDB itemDB;
    PlayerInventory inventory;
    GainsUI gainsUI;
    GameObject weaponSlot;
    Weapon equippedWeapon;

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        playerCharacter.LoadCharacterCreation();
        movement.z = transform.position.z;

        rb = GetComponent<Rigidbody2D>();

        animations = GetComponentInChildren<Animator>();

        playerLayer = LayerMask.NameToLayer("Player");
        environmentLayer = LayerMask.NameToLayer("Environment");
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        defaultGravity = rb.gravityScale;

        Physics2D.IgnoreLayerCollision(playerLayer, itemLayer, true);

        inventory = GetComponent<PlayerInventory>();
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();

        weaponSlot = GameObject.FindGameObjectWithTag("Weapon");

        gainsUI = GameObject.FindGameObjectWithTag("GainsCanvas").GetComponent<GainsUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        //Actions
        PerformActions();
        UpdateTimings();

        //Animations should probably be last so that it depends on everything else that happened this frame
        UpdateAnimation();

        if (Input.GetKeyDown(KeyCode.D))
        {
            inventory.DebugInv();
        }
    }
    private bool CanMove()
    {
        if (!climbing && canAttack)
        {
            return true;
        }

        return false;
    }

    private void HorizontalInput()
    {
        if (CanMove())
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                horInput = playerCharacter.MoveSpeed;
                transform.rotation = facingRight;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                horInput = -playerCharacter.MoveSpeed;
                transform.rotation = facingLeft;
            }
            else
            {
                horInput = 0.0f;
            }

            rb.velocity = new Vector2(horInput * Time.deltaTime, rb.velocity.y);
        }
    }

    private void VerticalInput()
    {
        if (touchingRope)
        {
            if (!climbing && Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                InitClimb();
            }
            if (climbing)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    rb.velocity = new Vector2(0, playerCharacter.ClimbSpeed * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    Debug.Log("Hello");
                    rb.velocity = new Vector2(0, -playerCharacter.ClimbSpeed * Time.deltaTime);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }

    private void InitClimb()
    {
        climbing = true;
        rb.gravityScale = 0.0f;
        rb.velocity = Vector2.zero;
        Physics2D.IgnoreLayerCollision(playerLayer, environmentLayer, true);
    }

    private void Jump()
    {
        if ((grounded || climbing) && Input.GetKey(KeyCode.Space))
        {
            grounded = false;
            rb.velocity = Vector2.zero; //prevents the player from shooting up into the sky if they were already moving up on the rope
            if (climbing)
            {
                climbing = false;
                rb.gravityScale = defaultGravity;
                Physics2D.IgnoreLayerCollision(playerLayer, environmentLayer, false);
            }
            Vector2 jump = new Vector2(0, playerCharacter.JumpSpeed);
            rb.AddForce(jump, ForceMode2D.Impulse);
        }
    }
    
    private void UpdateAnimation()
    {
        animations.SetBool("Grounded", grounded);
        if (grounded)
        {
            animations.SetFloat("Speed", Mathf.Abs(horInput));
        }
    }

    private void Attack()
    {
        if (canAttack)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                movement = Vector3.zero; //stop all movement
                animations.SetBool("Attacking", true);
                BasicAttack();
                canAttack = false;
            }
        }
    }

    private void BasicAttack()
    {
        float attackRange = playerCharacter.BaseAttackRange;
        int attackDamage = playerCharacter.GetDamage();

        if (equippedWeapon)
        {
            attackRange = equippedWeapon.AttackRange;
            attackDamage = equippedWeapon.Damage * playerCharacter.GetMainAPPoints();
        }

        RaycastHit2D hit = Physics2D.Raycast(weaponSlot.transform.position, -weaponSlot.transform.right, attackRange, enemyLayer);

        if (hit.transform != null)
        {
            EnemyController enemyHit = hit.transform.gameObject.GetComponent<EnemyController>();

            int expGained = enemyHit.TakeDamage(attackDamage, -weaponSlot.transform.right);

            if(expGained > 0)
            {
                playerCharacter.Experience += expGained;
                gainsUI.AddGain(expGained.ToString(), "XP");
            }

        }

    }

    private void PickupItem()
    {
        if (itemTouching != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ItemID item = itemTouching.GetComponent<ItemID>();

                gainsUI.AddGain(itemDB.GetItemName(item.itemID), "Item");
                inventory.AddToInventory(item.itemID);
                item.Pickup();
            }
        }
    }

    private void UpdateAttackTime()
    {
        float attackSpeed = playerCharacter.BaseAttackSpeed;
        if (equippedWeapon)
        {
            attackSpeed = equippedWeapon.AttackSpeed;
        }

        if (!canAttack)
        {
            attackTime += Time.deltaTime;

            if (attackTime >= (1 / attackSpeed))
            {
                attackTime = 0;
                canAttack = true;
                animations.SetBool("Attacking", false);
            }
        }
    }

    private void UpdateDamageTime()
    {
        if (damaged)
        {
            damageTime += Time.deltaTime;

            if (damageTime >= 1)
            {
                damageTime = 0;
                damaged = false;
            }
        }
    }

    private void Movement()
    {
        HorizontalInput();
        VerticalInput();
        Jump();
    }

    private void PerformActions()
    {
        Attack();
        PickupItem();
        ToggleInventory();
        ToggleCharacterUI();
    }

    private void UpdateTimings()
    {
        UpdateDamageTime();
        UpdateAttackTime();
    }

    private void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.UI.Show(!inventory.Opened);
        }
    }

    private void ToggleCharacterUI()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerCharacter.UI.Show(!playerCharacter.Opened);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        int layer = collision.gameObject.layer;


        if (tag.Equals("Platform"))
        {
            if (!climbing)
            {
                grounded = true;
                movement.y = 0.0f;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        int layer = collision.gameObject.layer;

        if (layer == environmentLayer)
        {
            grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Rope"))
        {
            touchingRope = true;
        }
        else if (tag.Equals("Enemy"))
        {
            EnemyCharacter enemy = collision.gameObject.GetComponentInParent<EnemyCharacter>();
            damaged = true;

            rb.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
            playerCharacter.TakeDamage(enemy.TouchDamage);
        }
        else if (tag.Equals("Item"))
        {
            itemTouching = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Enemy") && !damaged)
        {
            EnemyCharacter enemy = collision.gameObject.GetComponentInParent<EnemyCharacter>();
            damaged = true;

            rb.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
            playerCharacter.TakeDamage(enemy.TouchDamage);
        }
        else if (tag.Equals("Item"))
        {
            itemTouching = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.Equals("Rope"))
        {
            touchingRope = false;
            climbing = false;
            rb.gravityScale = defaultGravity;
            Physics2D.IgnoreLayerCollision(playerLayer, environmentLayer, false);
        }
        else if (tag.Equals("Item"))
        {
            itemTouching = null;
        }
    }
}
