using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOptionUI : MonoBehaviour
{
    UIController uiController;

    private void Awake()
    {
        uiController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelection()
    {
        int questID = GetComponent<QuestID>().ID;

        uiController.ToggleDialogueSelection(false, null);
        uiController.ToggleActualDialogue(true, questID);
    }
}
