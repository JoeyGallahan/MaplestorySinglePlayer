using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextEffects : MonoBehaviour
{
    [SerializeField] float seconds = 0.0f;
    float maxTime = 2.0f;
    EnemyDamageUI damageUI;
    // Start is called before the first frame update
    void Start()
    {
        damageUI = GetComponentInParent<EnemyDamageUI>();
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;

        if (seconds >= maxTime)
        {
            damageUI.damages.Dequeue();
            Destroy(this.gameObject);
        }
    }
}
