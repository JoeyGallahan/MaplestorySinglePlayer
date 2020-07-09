using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDoubleClick : MonoBehaviour
{
    int clicks = 0;
    float seconds = 0f;
    float maxDoubleClickTime = 0.3f;
    bool clicked = false;
    UIController uiController;

    private void Awake()
    {
        uiController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void OnMouseDown()
    {
        clicks++;
        clicked = true;

        if (clicks >= 2)
        {
            clicks = 0;
            seconds = 0;

            clicked = false;

            uiController.ToggleDialogueSelection(true, gameObject.GetComponent<NpcCharacter>().NPCID);
        }
    }
}
