using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    //Other instances we need to control the player
    PlayerCharacter playerCharacter;
    Animator animations;
    PhysicsObject physicsObject;
    UIController uiController;
    SkillDB skillDB;

    //Movement
    float horInput = 0.0f;
    float verInput = 0.0f;
    Quaternion facingLeft = Quaternion.Euler(Vector3.zero);
    Quaternion facingRight = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    //States
    [SerializeField] bool climbing = false;
    bool jumping = false;
    [SerializeField] bool touchingRope = false;

    //Taking Damage
    [SerializeField] float damageTime;
    [SerializeField] bool damaged = false;

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
        playerCharacter = GetComponent<PlayerCharacter>();
        physicsObject = GetComponent<PhysicsObject>();

        playerLayer = LayerMask.NameToLayer("Player");
        environmentLayer = LayerMask.NameToLayer("Environment");
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        inventory = GetComponent<PlayerInventory>();
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
        skillDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<SkillDB>();
        uiController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();

        weaponSlot = GameObject.FindGameObjectWithTag("Weapon");
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpecialCollisions();
        Movement();
        PerformActions();
        UpdateTimings();
    }

    private void FixedUpdate()
    {
        Jump(); //Jump is causing issues by going extremely high with only minor drops in framerate so it's going here until it can learn to behave
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
        
        physicsObject.SetVelX(horInput * Time.deltaTime);

        if (climbing)
        {
            physicsObject.SetVelY(verInput * Time.deltaTime);
        }
    }

    //Controls the player moving left and right
    private void HorizontalInput()
    {
        if (CanMove()) //If you can move
        {
            if (Input.GetKey(KeyCode.RightArrow)) //If you press the key to move right
            {
                horInput = playerCharacter.MoveSpeed; //Apply the movespeed to the horizontal input
                transform.rotation = facingRight; //Make the player face right
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) //If you press the key to move left
            {
                horInput = -playerCharacter.MoveSpeed; //Apply the movespeed to the horizontal input
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
        if (Input.GetKey(KeyCode.Space))
        {
            if (physicsObject.grounded || climbing) //Don't want to jump if you're not on the ground or climbing
            {
                jumping = false; //resets the jump variable for when you hit the ground
                climbing = false; //reset the climbing because you just jumped off a rope

                physicsObject.enableGravity = true; //Just in case you jumped off a rope

                physicsObject.grounded = false; //No longer on the ground
                physicsObject.SetVelY(playerCharacter.JumpSpeed * Time.deltaTime); //Jump
                jumping = true; //you're now jumping
            }
        }
    }

    //Initializes the variables and velocities for starting your climb
    private void InitClimbing()
    {
        climbing = true;
        physicsObject.enableGravity = false;
        physicsObject.grounded = false;
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
                verInput = playerCharacter.ClimbSpeed; 
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!climbing)
                {
                    InitClimbing();
                }

                verInput = -playerCharacter.ClimbSpeed;
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
                physicsObject.AddForce(new Vector3(0f, 20f * Time.deltaTime, 0f)); //Give the player a little boost so they can get up to the platform
            }

            verInput = 0f;
            physicsObject.enableGravity = true; //Reenable gravity
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
        if (canAttack) //If it is possible for you to do this
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

            if (expGained > 0) //If you actually gained some exp
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
                Debug.Log(itemTouching.name);
                uiController.AddGain(itemDB.GetItemName(item.itemID), "Item"); //Shows the item you picked up in the Gains UI
                inventory.AddToInventory(item.itemID); //Adds the item to the inventory
                item.Pickup(); //Removes the physical item from the scene
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
            float attackSpeed = playerCharacter.BaseAttackSpeed;

            if (playerCharacter.Equips.GetWeapon() != null)
            {
                attackSpeed = playerCharacter.Equips.GetWeapon().AttackSpeed;
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
        if (!damaged)
        {
            Debug.Log("Ouch");

            //Knockback
            physicsObject.grounded = false;
            physicsObject.SetVelY(0f);
            Vector2 knockbackForce = Vector2.up * playerCharacter.JumpSpeed * Time.deltaTime;
            physicsObject.AddForce(knockbackForce);

            playerCharacter.TakeDamage(enemy.TouchDamage);
            damaged = true;
        }
    }
}
