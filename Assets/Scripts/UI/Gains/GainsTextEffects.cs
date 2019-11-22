using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GainsTextEffects : MonoBehaviour
{
    [SerializeField] float seconds = 0.0f;
    float maxTime = 3.0f;
    GainsUI gainsUI;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        gainsUI = GetComponentInParent<GainsUI>();
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;

        if (seconds >= maxTime)
        {
            gainsUI.gains.Dequeue();
            Destroy(this.gameObject);
        }
    }
}
