using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDoubleClick : MonoBehaviour, IPointerDownHandler
{
    int clicks = 0;
    float seconds = 0f;
    float maxDoubleClickTime = 0.3f;
    bool clicked = false;

    ItemID item;
    ItemDB db;

    public void Awake()
    {
        item = GetComponent<ItemID>();
        db = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
    }

    public void Update()
    {
        DoubleClick();
    }

    private void DoubleClick()
    {
        if (clicked)
        {
            seconds += Time.deltaTime;

            if (seconds >= maxDoubleClickTime)
            {
                clicks = 0;
                seconds = 0;
                clicked = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        clicks++;
        clicked = true;

        if (clicks >= 2)
        {
            clicks = 0;
            seconds = 0;

            clicked = false;

            db.GetItemByID(item.itemID).Action();
        }
    }
}
