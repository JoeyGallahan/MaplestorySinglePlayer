using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GainsUI : MonoBehaviour
{
    Canvas parentCanvas;
    [SerializeField] TextMeshProUGUI textPrefab;
    public Queue<TextMeshProUGUI> gains = new Queue<TextMeshProUGUI>();
    int maxCount = 5;
    Vector3 offset = new Vector3(0.0f, 30.0f);

    private void Awake()
    {
        parentCanvas = GetComponent<Canvas>();
    }
    
    public void AddGain(string amount, string type)
    {
        TextMeshProUGUI newGain = textPrefab;

        string pretext = "+";
        string posttext = "";

        switch(type)
        {
            case "XP": posttext = " xp";
                break;
            case "Item": posttext = " (1)";
                break;
        }

        newGain.SetText(pretext + amount + posttext);
        
        Vector3 position = newGain.transform.position;
        
        if (gains.Count > 0)
        {
            int ctr = gains.Count;
            foreach(TextMeshProUGUI t in gains)
            {
                t.transform.localPosition = position + offset * ctr;
                ctr--;
            }
        }

        TextMeshProUGUI temp = Instantiate(newGain, position, Quaternion.identity);
        temp.transform.SetParent(parentCanvas.transform, false);
        temp.transform.localPosition = position;

        gains.Enqueue(temp);

        if (gains.Count > maxCount)
        {
            Destroy(gains.Dequeue().gameObject);
        }
    }
}
