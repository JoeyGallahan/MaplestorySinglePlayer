using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    //Other instances we need to control the player
    PlayerCharacter playerCharacter;
    Animator animations;
    PhysicsObject physicsObject;

    float horInput = 0.0f;
    float verInput = 0.0f;

    Quaternion facingLeft = Quaternion.Euler(Vector3.zero);
    Quaternion facingRight = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    [SerializeField] bool climbing = false;
    bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        animations = GetComponentInChildren<Animator>();
        playerCharacter = GetComponent<PlayerCharacter>();
        physicsObject = GetComponent<PhysicsObject>();


        //playerCharacter.LoadCharacterCreation();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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
        
        physicsObject.velocity.x = horInput * Time.deltaTime;

        if (climbing)
        {
            physicsObject.velocity.y = verInput * Time.deltaTime;
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
                physicsObject.enableGravity = true;

                physicsObject.grounded = false; //No longer on the ground
                physicsObject.velocity.y = playerCharacter.JumpSpeed * Time.deltaTime; //Jump af
                jumping = true; //you're now jumping
            }
        }
    }

    //Controls the player moving up and down a rope
    private void VerticalInput()
    {
        if (physicsObject.touchingRope)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                climbing = true;
                physicsObject.enableGravity = false;
                physicsObject.grounded = false;

                physicsObject.velocity.y = 0f;
                verInput = playerCharacter.ClimbSpeed; 
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                climbing = true;
                physicsObject.enableGravity = false;
                physicsObject.grounded = false;
                physicsObject.velocity.y = 0f;

                verInput = -playerCharacter.ClimbSpeed;
            }
            else
            {
                verInput = 0f;
            }
        }
        else if (climbing)
        {
            climbing = false;
            physicsObject.velocity.y = 0f;

            if (verInput > 0) //If you were climbing up
            {
                physicsObject.AddForce(new Vector3(0f, 20f * Time.deltaTime, 0f)); //Give the player a little boost so they can get up to the platform
            }

            verInput = 0f;
            physicsObject.enableGravity = true;
        }
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

    //Updates the players jumping, walking, and idle animations
    private void UpdateAnimation()
    {
        animations.SetBool("Grounded", physicsObject.grounded);
        if (physicsObject.grounded)
        {
            animations.SetFloat("Speed", Mathf.Abs(horInput));
        }
    }

}
