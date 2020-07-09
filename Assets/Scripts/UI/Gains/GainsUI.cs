using System;
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

    Action<EventParam> expGainListener;
    Action<EventParam> itemGainListener;

    private void Awake()
    {
        parentCanvas = GetComponent<Canvas>();

        expGainListener = new Action<EventParam>(OnEXPGainEvent);
        EventManager.AddListener("EXP_GAIN", OnEXPGainEvent);

        itemGainListener = new Action<EventParam>(OnItemGainEvent);
        EventManager.AddListener("ITEM_PICKUP", OnItemGainEvent);
    }

    private void OnEXPGainEvent(EventParam param)
    {
        AddGain(param.paramInt, "XP");
    }

    private void OnItemGainEvent(EventParam param)
    {
        string item = ItemDB.Instance.GetItemName(param.paramItemID);
        Debug.Log("Item name " + item);

        AddGain(param.paramInt, item);
    }

    private void AddGain(int amount, string item = "")
    {
        string pretext = "+";
        string posttext = "";
                       
        Vector3 position = textPrefab.transform.position;
        
        if (gains.Count > 0)
        {
            int ctr = gains.Count;
            foreach(TextMeshProUGUI t in gains)
            {
                t.transform.localPosition = position + offset * ctr;
                ctr--;
            }
        }

        TextMeshProUGUI temp = Instantiate(textPrefab, position, Quaternion.identity);
        temp.transform.SetParent(parentCanvas.transform, false);
        temp.transform.localPosition = position;

        switch (item)
        {
            case "XP":
                posttext = " xp";
                temp.SetText(pretext + amount.ToString() + posttext);
                break;
            default:
                posttext = " (" + amount.ToString() + ")";
                temp.SetText(pretext + item + posttext);
                break;
        }

        gains.Enqueue(temp);

        if (gains.Count > maxCount)
        {
            Destroy(gains.Dequeue().gameObject);
        }
    }
}
