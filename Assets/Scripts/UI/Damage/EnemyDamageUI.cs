using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDamageUI : MonoBehaviour
{
    [SerializeField] TextMeshPro textPrefab;

    Vector3 offset = new Vector3(0.0f, 1.5f);
    public Queue<TextMeshPro> damages = new Queue<TextMeshPro>();
    int maxCount = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDamage(int amount)
    {
        TextMeshPro damageText = textPrefab;

        damageText.SetText(amount.ToString());

        Vector3 position = damageText.transform.position + offset;

        if (damages.Count > 0)
        {
            int ctr = damages.Count;
            foreach (TextMeshPro t in damages)
            {
                t.transform.localPosition = position + offset * ctr;
                ctr--;
            }
        }

        TextMeshPro temp = Instantiate(damageText, position, Quaternion.identity);
        temp.transform.SetParent(this.transform, false);
        temp.transform.localPosition = position;

        damages.Enqueue(temp);

        if (damages.Count > maxCount)
        {
            Destroy(damages.Dequeue().gameObject);
        }
    }
}
