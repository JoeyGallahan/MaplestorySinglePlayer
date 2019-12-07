using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformSnap : MonoBehaviour
{
    [SerializeField] Vector2 gridSize = new Vector2(2f, 0.25f);
    // Update is called once per frame
    void Update()
    {
        Vector3 snapPos;

        snapPos.x = Mathf.RoundToInt(transform.position.x / gridSize.x) * gridSize.x;
        snapPos.y = Mathf.RoundToInt(transform.position.y / gridSize.y) * gridSize.y;
        snapPos.z = transform.position.z;
        //snapPos.z = Mathf.RoundToInt(transform.position.z / gridSize) * gridSize;

        transform.SetPositionAndRotation(snapPos, transform.rotation);
    }
}
