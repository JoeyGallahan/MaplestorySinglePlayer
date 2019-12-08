using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    TeleportID id;
    public Teleport otherTeleportPlat;
    public Vector3 cameraLoc;
    [SerializeField] float cameraOffsetY;

    private void Awake()
    {
        id = GetComponent<TeleportID>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetTeleportMatch();
        cameraLoc = transform.position;
        cameraLoc.y = cameraOffsetY;
        cameraLoc.z = -10f;
    }

    private void GetTeleportMatch()
    {
        GameObject[] teles = GameObject.FindGameObjectsWithTag("TeleportPlat");

        foreach(GameObject t in teles)
        {
            TeleportID teleID = t.GetComponent<TeleportID>();
            if (t != this.gameObject && teleID.teleportID == id.teleportID)
            {
                otherTeleportPlat = t.GetComponent<Teleport>();

                return;
            }
        }
    }
}
