using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject objDragged;
    ItemID itemID;
    ItemDB itemDB;
    Transform parent;
    [SerializeField] GameObject[] hotkeys;

    private void Awake()
    {
        itemID = GetComponent<ItemID>();
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();

        hotkeys = GameObject.FindGameObjectsWithTag("Hotkey");

        parent = GameObject.FindGameObjectWithTag("HotkeyWrapper").transform;
    }

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        objDragged = Instantiate(this.gameObject, parent);
        objDragged.transform.SetParent(parent, false);
        objDragged.GetComponent<RectTransform>().anchorMin = new Vector2 (0.5f, 0.5f);
        objDragged.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
    }
    #endregion

    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        objDragged.transform.position = Input.mousePosition;
    }
    #endregion

    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        CheckForOverlap();
        if (parent.tag.Equals("Hotkey"))
        {
            objDragged.transform.SetParent(parent, false);
            objDragged.transform.localPosition = Vector3.zero;
        }
        else
        {
            Destroy(objDragged);
        }

        parent = GameObject.FindGameObjectWithTag("HotkeyWrapper").transform;
    }
    #endregion

    private void CheckForOverlap()
    {
        Rect rect1 = new Rect(objDragged.GetComponent<RectTransform>().localPosition.x, objDragged.GetComponent<RectTransform>().localPosition.y, objDragged.GetComponent<RectTransform>().rect.width, objDragged.GetComponent<RectTransform>().rect.height);

        foreach (GameObject g in hotkeys)
        {
            Rect rect2 = new Rect(g.GetComponent<RectTransform>().localPosition.x, g.GetComponent<RectTransform>().localPosition.y, g.GetComponent<RectTransform>().rect.width, g.GetComponent<RectTransform>().rect.height);

            if (g.name == "T")
            {
                Debug.Log(rect1.Overlaps(rect2) + " " + g.name + "- Rect1(" + rect1.x + "," + rect1.y + ") Rect2(" + rect2.x + "," + rect2.y + ")");
            }

            //Make sure it's not overlapping itself
            if (!rect1.Equals(rect2) && rect1.Overlaps(rect2, true))
            {
                parent = g.transform;
            }
        }
    }
}
