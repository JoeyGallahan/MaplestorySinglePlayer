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
    [SerializeField] bool touchingTeleport = false;

    //Movement and rotations
    Vector3 movement = Vector3.zero;
    float horInput = 0.0f;
    Quaternion facingLeft = Quaternion.Euler(Vector3.zero);
    Quaternion facingRight = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    GameObject nextTeleportLocation;
    Vector3 teleportCameraLoc;

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

    UIController uiController;
    SkillDB skillDB;

    bool teleporting = false;

    //Inventory
    [SerializeField] GameObject itemTouching;
    ItemDB itemDB;
    PlayerInventory inventory;
    GameObject weaponSlot;

    private void Awake()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        movement.z = transform.position.z;

        rb = GetComponent<Rigidbody2D>();

        animations = GetComponentInChildren<Animator>();

        playerLayer = LayerMask.NameToLayer("Player");
        environmentLayer = LayerMask.NameToLayer("Environment");
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        defaultGravity = rb.gravityScale;

        Physics2D.IgnoreLayerCollision(playerLayer, itemLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, 13, true);

        inventory = GetComponent<PlayerInventory>();
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
        skillDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<SkillDB>();
        uiController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();

        weaponSlot = GameObject.FindGameObjectWithTag("Weapon");

    }
    private void Start()
    {
        playerCharacter.LoadCharacterCreation();
    }

    // Update is called once per frame
    void Update()
    {
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

    private void FixedUpdate()
    {
        Movement();
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

            rb.velocity = new Vector2(horInput, rb.velocity.y);
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
                    rb.velocity = new Vector2(0, playerCharacter.ClimbSpeed);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    rb.velocity = new Vector2(0, -playerCharacter.ClimbSpeed);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }

    //Initializes climbing
    private void InitClimb()
    {
        climbing = true;  //you are now climbing
        rb.gravityScale = 0.0f; //you're no longer affected by gravity
        rb.velocity = Vector2.zero; //You have no more velocity. just boom. stopped.
        Physics2D.IgnoreLayerCollision(playerLayer, environmentLayer, true); //Ignore collision with platforms
    }

    //Makes the player jump
    private void Jump()
    {
        //If you're on the ground or climbing and you press the jump key
        if ((grounded || climbing) && Input.GetKey(KeyCode.Space))
        {
            grounded = false; //You're no longer grounded
            rb.velocity = Vector2.zero; //prevents the player from shooting up into the sky if they were already moving up on the rope

            if (climbing) //If you were climbing
            {
                climbing = false; //You're not climbing anymore
                rb.gravityScale = defaultGravity; //Reset the gravity 
                Physics2D.IgnoreLayerCollision(playerLayer, environmentLayer, false); //You can collide with platforms again
            }

            Vector2 jump = new Vector2(0, playerCharacter.JumpSpeed); //Get a nice force for your jump
            rb.AddForce(jump, ForceMode2D.Impulse); //apply the force to the player
        }
    }
    
    //Updates the players jumping, walking, and idle animations
    private void UpdateAnimation()
    {
        animations.SetBool("Grounded", grounded);
        if (grounded)
        {
            animations.SetFloat("Speed", Mathf.Abs(horInput));
        }
    }

    //Determines if you can use a basic attack and will call the appropriate methods to do so
    private void Attack()
    {
        if (canAttack) //If it is possible for you to do this
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) //And you hit the proper keys
            {
                movement = Vector3.zero; //stop all movement
                animations.SetBool("Attacking", true); //Start the attack animation
                BasicAttack(); //Actually attack
                canAttack = false; //Can't immediately attack until the cooldown is up
            }
        }
    }

    //Performs a basic attack
    private void BasicAttack()
    {
        if (grounded)
        {
            //Make it so they can't move horizontally while attacking
            Vector3 newVel = Vector3.zero;
            newVel.y = rb.velocity.y;
            rb.velocity = newVel;
        }

        float attackRange = playerCharacter.BaseAttackRange; //Get your base attack range
        int attackDamage = playerCharacter.GetDamage(); //Get your base damage

        if (playerCharacter.Equips.GetWeapon() != null) //If you have a weapon equipped
        {
            attackRange = playerCharacter.Equips.GetWeapon().AttackRange; //Update the range to the range of the weapon.
            //We dont need to update the attack damage because that's already accounted for in the GetDamage() method
        }

        RaycastHit2D hit = Physics2D.Raycast(weaponSlot.transform.position, -weaponSlot.transform.right, attackRange, enemyLayer); //Find an enemy within range

        //If you hit an enemy
        if (hit.transform != null)
        {
            EnemyController enemyHit = hit.transform.gameObject.GetComponent<EnemyController>(); //Get the enemy controller

            int expGained = enemyHit.TakeDamage(attackDamage, -weaponSlot.transform.right); //Make the enemy take damage and get the exp earned from this

            if(expGained > 0) //If you actually gained some exp
            {
                playerCharacter.Experience += expGained; //Add it to your player
                uiController.AddGain(expGained.ToString(), "XP"); //Show it on the Gains UI
            }

        }

    }

    //Pickup the item you are touching
    private void PickupItem()
    {
        if (itemTouching != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ItemID item = itemTouching.GetComponent<ItemID>();

                uiController.AddGain(itemDB.GetItemName(item.itemID), "Item"); //Shows the item you picked up in the Gains UI
                inventory.AddToInventory(item.itemID); //Adds the item to the inventory
                item.Pickup(); //Removes the physical item from the scene
            }
        }
    }

    //Updates the time since the player last used a basic attack. Essentially a cooldown.
    private void UpdateAttackTime()
    {
        float attackSpeed = playerCharacter.BaseAttackSpeed;
        if (playerCharacter.Equips.GetWeapon() != null)
        {
            attackSpeed = playerCharacter.Equips.GetWeapon().AttackSpeed;
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

    //Updates the time since you were last damaged by an enemy. This prevents the player from basically being spam-attacked
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

    //Calls the appropriate methods to update the player's movement
    private void Movement()
    {
        HorizontalInput();
        VerticalInput();
        Jump();
    }

    //Calls the appropriate methods to perform simple actions like basic attacks, picking up items, and moving to new maps
    private void PerformActions()
    {
        Attack();
        PickupItem();
        ActivateTeleport();
    }
    
    //Activates the teleporting sequence
    private void ActivateTeleport()
    {
        //If you aren't actively teleporting but you are on a teleport platform and hit the button
        if (!teleporting && touchingTeleport && Input.GetKeyDown(KeyCode.UpArrow))
        {
            uiController.FadeToBlack(); //Fade out
            teleporting = true; //You are now actively teleporting
        }
        else if (teleporting)
        {
            if (!uiController.FadingOut()) //Wait to move the player until you're all the way faded
            {
                Teleport(); //move the player
                teleporting = false; //no longer teleporting
            }
        }
    }

    //Actually teleports the player to the new area
    private void Teleport()
    {
        transform.position = nextTeleportLocation.transform.position; //move the player

        Camera.main.transform.position = teleportCameraLoc; //Snaps the camera to the player's new position

        uiController.FadeInFromBlack(); //Fade back in
    }

    //Updates the timings for simple things like taking damage and basic attacks
    private void UpdateTimings()
    {
        UpdateDamageTime();
        UpdateAttackTime();
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
        else if (tag.Equals("TeleportPlat"))
        {
            touchingTeleport = true;
            Teleport tele = collision.gameObject.GetComponent<Teleport>();

            nextTeleportLocation = tele.otherTeleportPlat;
            teleportCameraLoc = tele.cameraLoc; 
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
        else if (tag.Equals("TeleportPlat"))
        {
            touchingTeleport = true;
            Teleport tele = collision.gameObject.GetComponent<Teleport>();

            nextTeleportLocation = tele.otherTeleportPlat;
            teleportCameraLoc = tele.cameraLoc;
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
        else if (tag.Equals("TeleportPlat"))
        {
            touchingTeleport = false;
        }
    }
}
