using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /*
    Transform player;
    [SerializeField]Vector3 offset;
    Vector2 lastLocation;
    [SerializeField] float cameraMoveSpeed = 1.0f;
    [SerializeField] bool canFollowLeft = true;
    [SerializeField] bool canFollowRight = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        offset.z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newCamPos = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

        if (player.transform.localScale.x > 0f)
        {
            newCamPos = new Vector3(newCamPos.x + offset.x, newCamPos.y, newCamPos.z);
        }
        else
        {
            newCamPos = new Vector3(newCamPos.x - offset.x, newCamPos.y, newCamPos.z);
        }
        transform.position = Vector3.Lerp(transform.position, newCamPos, cameraMoveSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name + " - " + collision.tag);
        if (collision.tag.Equals("WallLeft"))
        {
            //Debug.Log("EnterLeft");
            canFollowLeft = false;
        }
        else if (collision.tag.Equals("WallRight"))
        {
            //Debug.Log("EnterRight");
            canFollowRight = false;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("WallLeft"))
        {
            //Debug.Log("ExitLeft");
            canFollowLeft = true;
        }
        else if (collision.tag.Equals("WallRight"))
        {
            //Debug.Log("ExitRight");
            canFollowRight = true;
        }
    }
    */

    public Vector2 focusAreaSize;
    FocusArea focusArea;
    public GameObject target;
    BoxCollider2D collider;

    public float verticalOffset;

    private void Start()
    {
        collider = target.GetComponent<BoxCollider2D>();
        focusArea = new FocusArea(collider.bounds, focusAreaSize);
    }

    private void LateUpdate()
    {
        focusArea.Update(collider.bounds);

        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector2 center;
        float left, right, top, bottom;
        public Vector2 velocity;

        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2.0f;
            right = targetBounds.center.x + size.x / 2.0f;

            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            center = new Vector2((left + right) / 2.0f, (top + bottom) / 2.0f);

            velocity = Vector2.zero;
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0.0f;
            float shiftY = 0.0f;

            //horizontal
            if(targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            //Vertical
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            left += shiftX;
            right += shiftX;
            bottom += shiftY;
            top += shiftY;

            center = new Vector2((left + right) / 2.0f, (top + bottom) / 2.0f);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
