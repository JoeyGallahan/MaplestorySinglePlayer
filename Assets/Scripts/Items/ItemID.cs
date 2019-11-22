using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemID : MonoBehaviour
{
    [SerializeField] public int itemID;

    public void Pickup()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
