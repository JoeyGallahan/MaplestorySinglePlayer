using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Other instances we need to control the player
    Animator animations;
    PhysicsObject physicsObject;
    UIController uiController;

    //Movement
    float horInput = 0.0f;
    float verInput = 0.0f;
    [SerializeField] float timeToJumpApex;
    [SerializeField] float maxJumpHeight;
    Quaternion facingRight = Quaternion.Euler(Vector3.zero);
    Quaternion facingLeft = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    float jumpSpeed;

     //Wow fancy polish
     [SerializeField] float jumpBufferTiming = 0.2f;
    [SerializeField] float maxJumpBuffer = 0.2f;
    [SerializeField] float coyoteTiming = 0.0f;
    [SerializeField] float maxCoyoteTime = 0.2f;

    //States
    [SerializeField] bool climbing = false;
    [SerializeField] bool jumping = false;
    [SerializeField] bool touchingRope = false;

    //Taking Damage
    float damageTime;
    bool damaged = false;

    //Basic Attack
    [SerializeField] bool canAttack = true;
    float attackTime;
    
    //Teleporting
    bool teleporting = false;
    bool touchingTeleport = false;
    Teleport nextTeleportLocation;

    //Inventory
    [SerializeField] GameObject itemTouching;
    ItemDB itemDB;
    PlayerInventory inventory;
    GameObject weaponSlot;

    //Layers
    int playerLayer;
    int environmentLayer;
    int itemLayer = 12;
    LayerMask enemyLayer;

    void Awake()
    {
        animations = GetComponentInChildren<Animator>();
        physicsObject = GetComponent<PhysicsObject>();

        playerLayer = LayerMask.NameToLayer("Player");
        environmentLayer = LayerMask.NameToLayer("Environment");
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        inventory = GetComponent<PlayerInventory>();
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
        uiController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();

        weaponSlot = GameObject.FindGameObjectWithTag("Weapon");
    }

    private void Start()
    {
        inventory.AddToInventory(0, 5);
        inventory.AddToInventory(4, 5);

        physicsObject.gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpSpeed = Mathf.Abs(physicsObject.gravity) * timeToJumpApex;
        switch (PlayerCharacter.Instance.ClassName)
        {
            case "Warrior":
                inventory.AddToInventory(3);
                break;
            case "Mage":
                inventory.AddToInventory(5);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PerformActions();
        UpdateTimings();
        if (canAttack)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (canAttack)
        {
            Movement();
            Jump();
        }
        else
        {
            physicsObject.SetVelX(0.0f);
        }
        CheckSpecialCollisions();
    }

    private void LateUpdate()
    {
        UpdateAnimation();
    }

    //Perform all actions related to movement
    private void Movement()
    {
        HorizontalInput();
        VerticalInput();
        
        physicsObject.SetVelX(horInput);

        if (climbing)
        {
            physicsObject.SetVelY(verInput);
        }
    }

    //Controls the player moving left and right
    private void HorizontalInput()
    {
        if (CanMove()) //If you can move
        {
            if (Input.GetKey(KeyCode.RightArrow)) //If you press the key to move right
            {
                horInput = PlayerCharacter.Instance.MoveSpeed * Time.fixedDeltaTime; //Apply the movespeed to the horizontal input
                transform.rotation = facingRight; //Make the player face right
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) //If you press the key to move left
            {
                horInput = -PlayerCharacter.Instance.MoveSpeed * Time.fixedDeltaTime; //Apply the movespeed to the horizontal input
                transform.rotation = facingLeft; //Make the player face left
            }
            else
            {
                horInput = 0.0f; //If you havent pressed anything, immediately stop the player from moving
            }            
        }
    }

    //Controls the player jumping
    private void Jump()
    {        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTiming = 0.0f;

            if (climbing)
            {
                jumping = false; //resets the jump variable for when you hit the ground
                climbing = false; //reset the climbing because you just jumped off a rope

                physicsObject.enableGravity = true; //Just in case you jumped off a rope
                physicsObject.disableGroundedCheck = false;

                physicsObject.grounded = false; //No longer on the ground
                //physicsObject.SetVelY(PlayerCharacter.Instance.JumpSpeed * Time.fixedDeltaTime); //Jump
                physicsObject.SetVelY(jumpSpeed);
                jumping = true; //you're now jumping
            }
        }

        if (!physicsObject.grounded && !climbing)
        {
            jumpBufferTiming += Time.deltaTime;
            coyoteTiming += Time.deltaTime;
        }
        else if (!climbing)
        {
            coyoteTiming = 0.0f;
            jumping = false;
        }

        if (!jumping && jumpBufferTiming <= maxJumpBuffer && coyoteTiming <= maxCoyoteTime)
        {
            climbing = false; //reset the climbing because you just jumped off a rope

            physicsObject.enableGravity = true; //Just in case you jumped off a rope

            physicsObject.grounded = false; //No longer on the ground
            //physicsObject.SetVelY(PlayerCharacter.Instance.JumpSpeed * Time.fixedDeltaTime); //Jump            
            //jumpSpeed = -(maxJumpHeight * timeToJumpApex)/physicsObject.gravity * Time.deltaTime;
            physicsObject.SetVelY(jumpSpeed);
            Debug.Log(jumpSpeed);
            jumping = true; //you're now jumping
        }
    }

    //Initializes the variables and velocities for starting your climb
    private void InitClimbing()
    {
        climbing = true;
        physicsObject.enableGravity = false;
        physicsObject.grounded = false;
        physicsObject.disableGroundedCheck = true;
        horInput = 0f;
        verInput = 0f;
        physicsObject.SetVelocity(Vector3.zero);
    }

    //Controls the player moving up and down a rope
    private void VerticalInput()
    {
        if (touchingRope)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (!climbing)
                {
                    InitClimbing();
                }
                verInput = PlayerCharacter.Instance.ClimbSpeed * Time.deltaTime; 
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!climbing)
                {
                    InitClimbing();
                }

                verInput = -PlayerCharacter.Instance.ClimbSpeed * Time.deltaTime;
            }
            else
            {
                verInput = 0f;
            }
        }
        else if (climbing) //If you're not touching the rope, but still technically climbing
        {
            climbing = false; //no longer climbing
            physicsObject.SetVelY(0f); //reset your velocity

            if (verInput > 0) //If you were climbing up
            {              
                physicsObject.AddForce(new Vector3(0f, PlayerCharacter.Instance.ClimbSpeed * Time.fixedDeltaTime / 3.0f, 0f)); //Give the player a little boost so they can get up to the platform
            }

            verInput = 0f;
            physicsObject.enableGravity = true; //Reenable gravity
            physicsObject.disableGroundedCheck = false;
        }
    }

    //Activates the teleporting sequence
    private void ActivateTeleport()
    {
        //If you aren't actively teleporting but you are on a teleport platform and hit the button
        if (!teleporting && touchingTeleport && Input.GetKeyDown(KeyCode.UpArrow))
        {
            uiController.FadeToBlack(); //Fade out
            teleporting = true; //You are now actively teleporting
            AudioController.Instance.PlayAudioClip(2);
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

        Camera.main.transform.position = nextTeleportLocation.cameraLoc; //Snaps the camera to the player's new position

        uiController.FadeInFromBlack(); //Fade back in
    }

    //Returns whether or not a player can move
    private bool CanMove()
    {
        if (!climbing)
        {
            return true;
        }
        return false;
    }

    private void PerformActions()
    {
        Attack();
        ActivateTeleport();
        PickupItem();
    }

    //Determines if you can use a basic attack and will call the appropriate methods to do so
    private void Attack()
    {
        if (canAttack && !climbing) //If it is possible for you to do this
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) //And you hit the proper keys
            {
                animations.SetBool("Attacking", true); //Start the attack animation
                BasicAttack(); //Actually attack
                canAttack = false; //Can't immediately attack until the cooldown is up
            }
        }
    }

    //Performs a basic attack
    private void BasicAttack()
    {
        if (physicsObject.grounded)
        {
            //Make it so they can't move horizontally while attacking
            Vector3 newVel = Vector3.zero;
            newVel.y = physicsObject.GetVelocity().y;
            physicsObject.SetVelocity(newVel);
        }

        float attackRange = PlayerCharacter.Instance.BaseAttackRange; //Get your base attack range
        int attackDamage = PlayerCharacter.Instance.GetDamage(); //Get your base damage

        if (PlayerCharacter.Instance.Equips.GetWeapon() != null) //If you have a weapon equipped
        {
            attackRange = PlayerCharacter.Instance.Equips.GetWeapon().AttackRange; //Update the range to the range of the weapon.
                                                                                    //We dont need to update the attack damage because that's already accounted for in the GetDamage() method
        }

        RaycastHit2D hit = Physics2D.Raycast(weaponSlot.transform.position, -weaponSlot.transform.right, attackRange, enemyLayer); //Find an enemy within range

        //If you hit an enemy
        if (hit.transform != null)
        {
            EnemyController enemyHit = hit.transform.gameObject.GetComponent<EnemyController>(); //Get the enemy controller
            
            enemyHit.TakeDamage(attackDamage, -weaponSlot.transform.right);
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
                item.Pickup(); //Removes the physical item from the scene

                EventParam itemParams = new EventParam();
                itemParams.paramItemID = item.itemID;
                itemParams.paramInt = 1;
                EventManager.TriggerEvent("ITEM_PICKUP", itemParams);
            }
        }
    }

    //Updates the players jumping, walking, and idle animations
    private void UpdateAnimation()
    {
        animations.SetBool("Grounded", physicsObject.grounded);
        if (physicsObject.grounded)
        {
            animations.SetFloat("Speed", Mathf.Abs(horInput));
        }
    }

    //Updates the timings for simple things like taking damage and basic attacks
    private void UpdateTimings()
    {
        UpdateDamageTime();
        UpdateAttackTime();
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

    //Check for collisions with things like ropes, enemies, items, etc
    private void CheckSpecialCollisions()
    {
        EnemyCharacter enemyCollided = null;
        GameObject itemTouchedThisFrame = null;
        bool touchingTeleportThisFrame = false;
        bool touchingRopeThisFrame = false;

        foreach(GameObject o in physicsObject.nonEnvironmentCollisions)
        {
            if (o != null)
            {
                if (o.transform.tag.Equals("Rope"))
                {
                    touchingRopeThisFrame = true;
                }
                else if (o.transform.tag.Equals("Enemy"))
                {
                    if (enemyCollided == null)
                    {
                        enemyCollided = o.GetComponent<EnemyCharacter>();
                        TakeTouchDamage(enemyCollided);
                    }
                }
                else if (o.transform.tag.Equals("TeleportPlat"))
                {
                    touchingTeleportThisFrame = true;
                    nextTeleportLocation = o.GetComponent<Teleport>().otherTeleportPlat;
                }
                else if (o.transform.tag.Equals("Item"))
                {
                    if (itemTouchedThisFrame == null)
                    {
                        itemTouchedThisFrame = o;
                    }
                }
            }
        }

        itemTouching = itemTouchedThisFrame;

        touchingTeleport = touchingTeleportThisFrame;
        touchingRope = touchingRopeThisFrame;
    }

    //Updates the time since the player last used a basic attack. Essentially a cooldown.
    private void UpdateAttackTime()
    {
        if (!canAttack)
        {
            float attackSpeed = PlayerCharacter.Instance.BaseAttackSpeed;

            if (PlayerCharacter.Instance.Equips.GetWeapon() != null)
            {
                attackSpeed = PlayerCharacter.Instance.Equips.GetWeapon().AttackSpeed;
            }

            attackTime += Time.deltaTime;

            if (attackTime >= (1 / attackSpeed))
            {
                attackTime = 0;
                canAttack = true;
                animations.SetBool("Attacking", false);
            }
        }
    }

    //Take damage and get knocked back. For when you touched an enemy
    private void TakeTouchDamage(EnemyCharacter enemy)
    {
        if (!damaged && enemy.CanDealDamage())
        {
            //Knockback
            physicsObject.grounded = false;
            physicsObject.SetVelY(0f);
            Vector2 knockbackForce = Vector2.up * PlayerCharacter.Instance.MoveSpeed * Time.fixedDeltaTime / 3.0f;
            knockbackForce.x = -Vector2.right.x * PlayerCharacter.Instance.MoveSpeed * Time.fixedDeltaTime / 3.0f;
            physicsObject.AddForce(knockbackForce);

            PlayerCharacter.Instance.TakeDamage(enemy.TouchDamage);
            damaged = true;
        }
    }
}
