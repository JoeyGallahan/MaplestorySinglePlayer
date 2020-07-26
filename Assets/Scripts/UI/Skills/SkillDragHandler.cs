using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject objDragged; //The UI object being dragged
    RectTransform draggedRect; //The rect of the UI object being dragged. Used for checking overlaps
    SkillID skillID; //The ID of the item being dragged
    Transform parent; //Where the item is being dragged onto
    HotkeyController hotkeyController; //For dragging onto a hotkey
    bool dragging = false; //Used so we can check overlaps more often.

    BoxCollider2D skillCollider;

    private void Awake()
    {
        skillID = GetComponent<SkillID>();

        hotkeyController = GameObject.FindGameObjectWithTag("HotkeyWrapper").GetComponent<HotkeyController>();

        parent = GameObject.FindGameObjectWithTag("HotkeyWrapper").transform;
    }

    private void Update()
    {
        if (dragging) //The OnDrag() method is only called while the mouse is moving, so this lets us check where it is while it's being held.
        {
            //CheckForOverlap();
        }
    }

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true; //you're now dragging an item

        //Create a copy of this item
        objDragged = Instantiate(this.gameObject, parent);
        objDragged.transform.SetParent(parent, false);
        objDragged.GetComponent<RectTransform>().anchorMin = new Vector2 (0.5f, 0.5f);
        objDragged.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        //objDragged.transform.localScale = new Vector3(0.5f,0.5f,1.0f);

        draggedRect = objDragged.GetComponent<RectTransform>(); //Update the rect of the dragged item so we can check for overlaps
    }
    #endregion

    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        objDragged.transform.position = Input.mousePosition; //Moves the UI element along with the mouse
    }
    #endregion

    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        CheckForOverlap(); //We want a very final check just to be sure
        
        //If you've landed on a hotkey slot
        if (parent.tag.Equals("Hotkey"))
        {
            hotkeyController.DraggedToHotkey(parent, skillID.skillID, Hotkey.HotkeyType.SKILL); //Update the UI with what we've just added
            objDragged.transform.SetParent(parent, false); //Set this objects parent to the hotkey slot
            objDragged.transform.localPosition = Vector3.zero; //Make it line up in the middle
            objDragged.transform.SetAsFirstSibling();
        }
        else 
        {
            Destroy(objDragged); //If you've dragged it somewhere else, nothing happens.
        }

        //Reset our variables for the next time
        parent = GameObject.FindGameObjectWithTag("HotkeyWrapper").transform;
        dragging = false;
    }
    #endregion

    private void CheckForOverlap()
    {
        //Creates a rect based on our RectTransform that we've stored
        Rect rect1 = new Rect(draggedRect.localPosition.x, draggedRect.localPosition.y, draggedRect.rect.width, draggedRect.rect.height);

        List<Rect> allOverlapping = new List<Rect>();

        //Checks all the hotkey slots for an overlap
        foreach (Rect rect2 in hotkeyController.hotkeyRects)
        {
            //Make sure it's not overlapping itself
            if (!rect1.Equals(rect2) && rect1.Overlaps(rect2, false))
            {
                allOverlapping.Add(rect2);
            }
        }

        if (allOverlapping.Count > 0)
        {
            int bestOverlap = GetBestOverlap(rect1, allOverlapping);
            parent = hotkeyController.hotkeyUIs[allOverlapping[bestOverlap]].transform; //Sets the parent of the dragged item to the transform of the hotkey slot
        }
    }

    private int GetBestOverlap(Rect skill, List<Rect> hotkeys)
    {
        int bestHotkeyindex = 0;

        for (int i = 0; i < hotkeys.Count; i++)
        {
            float amtOverlap = Vector2.Distance(skill.min, hotkeys[i].min) + Vector2.Distance(skill.max, hotkeys[i].max);
            float amtOverlapBest = Vector2.Distance(skill.min, hotkeys[bestHotkeyindex].min) + Vector2.Distance(skill.max, hotkeys[bestHotkeyindex].max);
            
            if (amtOverlap < amtOverlapBest)
            {
                bestHotkeyindex = i;
            }
        }

        return bestHotkeyindex;
    }
}