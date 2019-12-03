using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyController : MonoBehaviour
{
    [SerializeField] HotkeyUI qKey, wKey, eKey, rKey, oneKey, twoKey, threeKey, fourKey; //The UIs for all of the hotkey slots
    public List<Rect> hotkeyRects = new List<Rect>(); //A list of all the Rects for the hotkey slots. For use with the drag handlers
    public Dictionary<Rect, HotkeyUI> hotkeyUIs = new Dictionary<Rect, HotkeyUI>(); //Connects each Rect with its UI slot
    public Dictionary<string, HotkeyUI> hotkeysByName = new Dictionary<string, HotkeyUI>(); //Connects each UI slot with its name (Q, W, E, etc)

    ItemDB itemDB;

    private void Awake()
    {
        InitHotkeys(); 
        itemDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<ItemDB>();
    }

    //Adds the skill or item to a hotkey slot. To be used by the DragHandlers.
    public void DraggedToHotkey(Transform parent, int id, Hotkey.HotkeyType type)
    {
        hotkeysByName[parent.name].AddToKey(id, type); //Get the appropriate hotkey slot and add it.
    }

    //Stores all the hotkey information that we need. To be used at the very beginning
    private void InitHotkeys()
    {
        GameObject[] hotkeys = GameObject.FindGameObjectsWithTag("Hotkey"); //Get alllll the hotkey slots in the game

        int ctr = 0;
        foreach (GameObject g in hotkeys) //Go through each one and assign it to the proper variables
        {
            Rect rect = new Rect(g.GetComponent<RectTransform>().localPosition.x, g.GetComponent<RectTransform>().localPosition.y, g.GetComponent<RectTransform>().rect.width, g.GetComponent<RectTransform>().rect.height);
            
            hotkeyRects.Add(rect); //Adding the rect of this hotkey slot to the list
            hotkeyUIs.Add(rect, null); //Initializes the hotkeyUI slot so we can add in the correct one later

            switch (g.name)
            {
                case "Q":
                    qKey = g.GetComponent<HotkeyUI>(); //Get the HotkeyUI for the Q hotkey slot
                    hotkeyUIs[rect] = qKey; //Get the rect of the hotkeyUI
                    hotkeysByName.Add(g.name, qKey); //Add it to the list of hotkeys sorted by name (like 'Q', 'W', etc)
                    break;
                case "W":
                    wKey = g.GetComponent<HotkeyUI>();
                    hotkeyUIs[rect] = wKey;
                    hotkeysByName.Add(g.name, wKey);
                    break;
                case "E":
                    eKey = g.GetComponent<HotkeyUI>();
                    hotkeyUIs[rect] = eKey;
                    hotkeysByName.Add(g.name, eKey);
                    break;
                case "R":
                    rKey = g.GetComponent<HotkeyUI>();
                    hotkeyUIs[rect] = rKey;
                    hotkeysByName.Add(g.name, rKey);
                    break;
                case "1":
                    oneKey = g.GetComponent<HotkeyUI>();
                    hotkeyUIs[rect] = oneKey;
                    hotkeysByName.Add(g.name, oneKey);
                    break;
                case "2":
                    twoKey = g.GetComponent<HotkeyUI>();
                    hotkeyUIs[rect] = twoKey;
                    hotkeysByName.Add(g.name, twoKey);
                    break;
                case "3":
                    threeKey = g.GetComponent<HotkeyUI>();
                    hotkeyUIs[rect] = threeKey;
                    hotkeysByName.Add(g.name, threeKey);
                    break;
                case "4":
                    fourKey = g.GetComponent<HotkeyUI>();
                    hotkeyUIs[rect] = fourKey;
                    hotkeysByName.Add(g.name, fourKey);
                    break;
            }

            ctr++;
        }
    }
}
