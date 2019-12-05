using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
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
}
