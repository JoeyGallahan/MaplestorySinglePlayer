using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    PhysicsObject physicsObject;

    private void Awake()
    {
        physicsObject = GetComponent<PhysicsObject>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (physicsObject.grounded)
        {
            physicsObject.SetVelX(0f);
        }
    }
}
