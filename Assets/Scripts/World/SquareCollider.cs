using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCollider : MonoBehaviour
{
    BoxCollider2D collider;
    float rangeX, rangeY;
    [SerializeField]Dictionary<string, GameObject> collisions = new Dictionary<string, GameObject>()
    {
        { "Left", null },
        { "Right", null },
        { "Top", null },
        { "Bottom", null}
    };

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();

        rangeX = collider.bounds.size.x / 2;
        rangeY = collider.bounds.size.y / 2;
    }

    private void Update()
    {
        CheckForCollisions();
    }

    private void CheckForCollisions()
    {
        CheckLeftCollision();
        CheckRightCollision();
        CheckTopCollision();
        CheckBottomCollision();
    }

    private void CheckLeftCollision()
    {
    }
    private void CheckRightCollision()
    {

    }
    private void CheckTopCollision()
    {
        Vector3 startPos = collider.transform.position;
        startPos.y -= rangeY - 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(startPos, -collider.transform.up, rangeY);
        if (hit.transform != null)
        {
            collisions["Bottom"] = hit.transform.gameObject;
            Debug.Log("Hit: " + gameObject.name);
        }
    }
    private void CheckBottomCollision()
    {

    }


}
