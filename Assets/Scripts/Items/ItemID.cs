using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemID : MonoBehaviour
{
    [SerializeField] public int itemID;

    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(12,12);
    }

    public void Pickup()
    {
        Destroy(transform.gameObject);
    }
}
