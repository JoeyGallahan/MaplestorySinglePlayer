using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    TeleportID id;
    public GameObject otherTeleportPlat;
    public Vector3 cameraLoc;

    private void Awake()
    {
        id = GetComponent<TeleportID>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetTeleportMatch();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void GetTeleportMatch()
    {
        GameObject[] teles = GameObject.FindGameObjectsWithTag("TeleportPlat");

        foreach(GameObject t in teles)
        {
            TeleportID teleID = t.GetComponent<TeleportID>();
            if (t != this.gameObject && teleID.teleportID == id.teleportID)
            {
                otherTeleportPlat = t;

                Transform area = t.transform.parent;
                cameraLoc = area.position;
                cameraLoc.z = -10f;

                return;
            }
        }
    }
}
