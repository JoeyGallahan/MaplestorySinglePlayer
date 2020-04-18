using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public List<GameObject> nonEnvironmentCollisions;

    //Forces on the object
    [SerializeField] float gravity = -0.2f;
    [SerializeField] Vector3 velocity = Vector3.zero;

    //States
    private bool leftBlocked = false, rightBlocked = false;
    public bool grounded = false;
    [SerializeField] public bool enableGravity = true;

    //Raycasting for collision
    BoxCollider2D collider;
    [SerializeField] float collisionRange = 0.25f;
    RaycastOrigins raycastOrigins;
    [SerializeField] int horizontalRayCount = 4;
    [SerializeField] int verticalRayCount = 4;
    float horizontalRaySpacing, verticalRaySpacing;

    //Layers
    int environmentLayer;
    int itemLayer = 12;
    LayerMask enemyLayer;

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public bool LeftBlocked { get => leftBlocked; }
    public bool RightBlocked { get => rightBlocked; }

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();

        environmentLayer = 9;
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        nonEnvironmentCollisions = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();
        ResetCollisions();
        CheckBottomCollision();
        CheckTopCollision();
        CheckLeftCollision();
        CheckRightCollision();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        ApplyPhysics();
    }

    void ResetCollisions()
    {
        nonEnvironmentCollisions = new List<GameObject>();
    }

    //Checks to see if this object is colliding with something under it
    void CheckBottomCollision()
    {
        if (velocity.y <= 0) //Don't want to check for bottom collisions if you're moving up
        {
            bool groundedThisFrame = false;

            //We want to have multiple rays come out from the bottom, so go through this for the number of Vertical Rays we have enabled
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.bottomLeft + (Vector2.right * (verticalRaySpacing * i)); //Starting from the bottom left, space out the ray based on the spacing we have set up and use this as the origin
                RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, -Vector2.up); //Get all the hits from this origin
                Debug.DrawRay(rayOrigin, -Vector2.up * collisionRange, Color.blue); //Draw the ray for debugging purposes

                foreach (RaycastHit2D hit in hits) //Go through each hit individually
                {                   
                    if (hit.collider != collider && hit.distance <= collisionRange) //If we hit something that's not itself and within the collision distance (using distance in the actual raycast isn't accurate so we just check it here)
                    {
                        if (hit.collider.gameObject.layer == environmentLayer && hit.collider.tag.Equals("Platform")) //If it's in the environment layer
                        {
                            velocity.y = -hit.distance;//snaps the object to the ground exactly where the ray hit
                            groundedThisFrame = true; //it's now grounded
                        }
                        else
                        {
                            if (!nonEnvironmentCollisions.Contains(hit.collider.gameObject))
                            {
                                nonEnvironmentCollisions.Add(hit.collider.gameObject); //Add it to the list of special collisions so we can do whatever we need to do in our controllers
                            }
                        }
                    }
                }
            }
            grounded = groundedThisFrame;
        }
    }

    //Checks to see if this object is colliding with something above it
    void CheckTopCollision()
    {              
        //We want to have multiple rays come out from the top, so go through this for the number of Vertical Rays we have enabled
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft + (Vector2.right * (verticalRaySpacing * i)); //Starting from the top left, space out the ray based on the spacing we have set up and use this as the origin
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.up); //Get all the hits from this origin
            Debug.DrawRay(rayOrigin, Vector2.up * collisionRange, Color.red); //Draw the ray for debugging purposes

            foreach (RaycastHit2D hit in hits) //Go through each hit individually
            {
                if (hit.collider != collider && hit.distance <= collisionRange) //If we hit something that's not itself and within the collision distance (using distance in the actual raycast isn't accurate so we just check it here)
                {
                    if (hit.collider.gameObject.layer != environmentLayer) //If it's not in the environment layer
                    {
                        if (!nonEnvironmentCollisions.Contains(hit.collider.gameObject))
                        {
                            nonEnvironmentCollisions.Add(hit.collider.gameObject); //Add it to the list of special collisions so we can do whatever we need to do in our controllers
                        }
                    }
                }
            }
        }
    }

    //Checks to see if this object is colliding with something to the left of it
    void CheckLeftCollision()
    {
        bool leftBlockedThisFrame = false;

        //We want to have multiple rays come out from the left, so go through this for the number of Horizontal Rays we have enabled
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft + (Vector2.up * (horizontalRaySpacing * i)); //Starting from the bottom left, space out the ray based on the spacing we have set up and use this as the origin
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, -Vector2.right); //Get all the hits from this origin
            Debug.DrawRay(rayOrigin, -Vector2.right * collisionRange, Color.green); //Draw the ray for debugging purposes

            foreach (RaycastHit2D hit in hits) //Go through each hit individually
            {
                if (hit.collider != collider && hit.distance <= collisionRange) //If we hit something that's not itself and within the collision distance (using distance in the actual raycast isn't accurate so we just check it here)
                {
                    if (hit.collider.gameObject.layer != environmentLayer) //If it's not in the environment layer
                    {
                        if (!nonEnvironmentCollisions.Contains(hit.collider.gameObject))
                        {
                            nonEnvironmentCollisions.Add(hit.collider.gameObject); //Add it to the list of special collisions so we can do whatever we need to do in our controllers
                        }
                    }
                    else if (hit.collider.tag.Equals("Wall"))
                    {
                        leftBlockedThisFrame = true; //don't let this object move to the left
                    }
                }
            }
        }

        leftBlocked = leftBlockedThisFrame;
    }

    //Checks to see if this object is colliding with something to the left of it
    void CheckRightCollision()
    {
        bool rightBlockedThisFrame = false;
        //We want to have multiple rays come out from the left, so go through this for the number of Horizontal Rays we have enabled
        for (int i = 0; i < horizontalRayCount; i++)
        {

            Vector2 rayOrigin = raycastOrigins.bottomRight + (Vector2.up * (horizontalRaySpacing * i)); //Starting from the bottom left, space out the ray based on the spacing we have set up and use this as the origin
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.right); //Get all the hits from this origin
            Debug.DrawRay(rayOrigin, Vector2.right * collisionRange, Color.yellow); //Draw the ray for debugging purposes

            foreach (RaycastHit2D hit in hits) //Go through each hit individually
            {
                if (hit.collider != collider && hit.distance <= collisionRange) //If we hit something that's not itself and within the collision distance (using distance in the actual raycast isn't accurate so we just check it here)
                {
                    if (hit.collider.gameObject.layer != environmentLayer) //If it's not in the environment layer
                    {
                        if (!nonEnvironmentCollisions.Contains(hit.collider.gameObject))
                        {
                            nonEnvironmentCollisions.Add(hit.collider.gameObject); //Add it to the list of special collisions so we can do whatever we need to do in our controllers
                        }
                    }
                    else if (hit.collider.tag.Equals("Wall"))
                    {
                        rightBlockedThisFrame = true; //Don't let this object move to the right
                    }
                }
            }
        }
        rightBlocked = rightBlockedThisFrame;
    }

    //Sets the entire velocity to a given value
    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    //Sets only the x value of the velocity
    public void SetVelX(float x)
    {
        velocity.x = x;
    }

    //Adds a force to the object
    public void AddForce(Vector3 force)
    {
        velocity += force;
    }

    //Sets only the y value of the velocity
    public void SetVelY(float y)
    {
        velocity.y = y;
    }

    //Moves the object based on its velocity
    void ApplyPhysics()
    {
        if (velocity.x < 0f && leftBlocked)
        {
            velocity.x = 0f;
        }
        else if (velocity.x > 0f && rightBlocked)
        {
            velocity.x = 0f;
        }
        transform.position += (velocity);
    }

    //Applies gravity to the object
    void ApplyGravity()
    {
        if (enableGravity && !grounded) //If gravity is enabled and the object is not on the ground
        {
            if (velocity.y > gravity) //If the object's velocity is higher than the force of gravity
            {
                AddForce(new Vector3(0f, gravity * Time.deltaTime, 0f)); //Add the gravity force to the object 
            }
            else
            {
                velocity.y = (gravity); //otherwise, make it move down at the speed of gravity
            }
        }
    }

    //Gets the outer bounds of this object
    Bounds GetBounds()
    {
        return collider.bounds;
    }

    //Updates the locations of where we want the raycasts to come out of
    void UpdateRaycastOrigins()
    {
        Bounds bounds = GetBounds();

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    //Calculates how far apart we want each ray to be
    void CalculateRaySpacing()
    {
        Bounds bounds = GetBounds();

        //Need at least two in each direction (each corner) so clamp it just in case im dumb and set it lower somewhere
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1); //Horizontal rays are spaced out by the y axis and we want them spread evenly along the collider
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1); //Vertical rays are spaced out by the x axis and we want them spread evenly along the collider
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }
}
