using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] float gravity = -0.05f;
    [SerializeField] public bool grounded = false;
    [SerializeField] BoxCollider2D collider;
    [SerializeField] float collisionRange = 2f;
    [SerializeField] public bool enableGravity = true;
    [SerializeField] public Vector3 velocity = Vector3.zero;
    public bool touchingRope = false;

    RaycastOrigins raycastOrigins;
    [SerializeField] int horizontalRayCount = 4;
    [SerializeField] int verticalRayCount = 4;
    float horizontalRaySpacing, verticalRaySpacing;
    float heightBeforeGrounded;

    //Layers
    int environmentLayer;
    int itemLayer = 12;
    LayerMask enemyLayer;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();

        environmentLayer = 9;
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        //DrawDebugRays();               
    }

    private void FixedUpdate()
    {
        UpdateRaycastOrigins();
        CalculateRaySpacing();
        ApplyGravity();
        ApplyPhysics();
        CheckBottomCollision();
        CheckTopCollision();
    }

    //Checks to see if this object is colliding with something under it
    void CheckBottomCollision()
    {
        if (velocity.y <= 0) //Don't want to check for bottom collisions if you're moving up
        {
            //Bottom Left Collisions
            RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigins.bottomLeft, -Vector2.up); //Get all the hits on the bottom left side

            foreach (RaycastHit2D hit in hits) //Go through each hit individually
            {
                if (hit.collider != collider && Vector2.Distance(hit.point, raycastOrigins.bottomLeft) <= collisionRange) //If we hit something that's not itself and within the collision distance
                {
                    if (hit.collider.gameObject.layer == environmentLayer) //If it's in the environment layer
                    {
                        if (hit.collider.gameObject.tag == "Rope")
                        {

                        }
                        else
                        {
                            grounded = true;
                            velocity.y = 0.0f;
                        }
                    }
                    return; //don't need to check anymore so stop the method
                }
            }

            hits = null; //reset the hits just to be safe

            //Bottom Right Collisions
            hits = Physics2D.RaycastAll(raycastOrigins.bottomRight, -Vector2.up); //Get all the hits on the bottom right side

            //Do the same thing but on the right side
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != collider && Vector2.Distance(hit.point, raycastOrigins.bottomRight) <= collisionRange)
                {
                    if (hit.collider.gameObject.layer == environmentLayer)
                    {
                        if (hit.collider.gameObject.tag == "Rope")
                        {

                        }
                        else
                        {
                            grounded = true;
                            velocity.y = 0.0f;
                        }
                    }
                    return;
                }
            }

            grounded = false;
        }
    }

    //Checks to see if this object is colliding with something above it
    void CheckTopCollision()
    {          
        //Top Left Side
        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigins.topLeft, -Vector2.up);
        Debug.DrawRay(raycastOrigins.topLeft, Vector2.up * collisionRange, Color.blue);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != collider && Vector2.Distance(hit.point, raycastOrigins.topLeft) <= collisionRange)
            {
                if (hit.collider.gameObject.layer != environmentLayer)
                {
                    if (hit.collider.gameObject.tag == "Rope")
                    {
                        touchingRope = true;
                        return;
                    }
                    else
                    {
                        touchingRope = false;
                    }
                }
            }
        }

        hits = null;

        //Top right side
        hits = Physics2D.RaycastAll(raycastOrigins.topRight, -Vector2.up);
        Debug.DrawRay(raycastOrigins.topRight, Vector2.up * collisionRange, Color.blue);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != collider && Vector2.Distance(hit.point, raycastOrigins.topRight) <= collisionRange)
            {
                if (hit.collider.gameObject.layer == environmentLayer)
                {
                    if (hit.collider.gameObject.tag == "Rope")
                    {
                        touchingRope = true;
                        return;
                    }
                    else
                    {
                        touchingRope = false;
                    }
                }
            }
        }

        touchingRope = false;
    }

    public void SetVelocity(Vector3 vel)
    {
        velocity = vel;
    }

    void ApplyPhysics()
    {
        transform.position += velocity;
    }

    public void AddForce(Vector3 force)
    {
        velocity += force;
    }

    void ApplyGravity()
    {
        if (enableGravity && !grounded)
        {
            if (velocity.y > gravity)
            {
                AddForce(new Vector3(0f, gravity * Time.deltaTime, 0f));
            }
            else
            {
                velocity.y = gravity;
            }
        }
    }

    Bounds GetBounds()
    {
        return collider.bounds;
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = GetBounds();

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = GetBounds();

        //Need at least two in each direction so clamp it just in case im dumb and set it lower somewhere
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    void DrawDebugRays()
    {
        Debug.DrawRay(raycastOrigins.bottomLeft, Vector2.up * -collisionRange, Color.red);
        Debug.DrawRay(raycastOrigins.bottomLeft, Vector2.right * -collisionRange, Color.red);

        Debug.DrawRay(raycastOrigins.bottomRight, Vector2.up * -collisionRange, Color.blue);
        Debug.DrawRay(raycastOrigins.bottomRight, Vector2.right * collisionRange, Color.blue);

        Debug.DrawRay(raycastOrigins.topLeft, Vector2.up * collisionRange, Color.green);
        Debug.DrawRay(raycastOrigins.topLeft, Vector2.right * -collisionRange, Color.green);

        Debug.DrawRay(raycastOrigins.topRight, Vector2.up * collisionRange, Color.white);
        Debug.DrawRay(raycastOrigins.topRight, Vector2.right * collisionRange, Color.white);
    }
}
