using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]Button rightButton;
    TextMeshProUGUI dialogueLineText;
    QuestList dialogueDB;
    DialogueScene currentScene;
    DialogueResponse currentResponse;
    int currentLineIndex = 0;

    private void Awake()
    {
        dialogueLineText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<QuestList>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseDialogue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseDialogue()
    {
        gameObject.SetActive(false);
        currentLineIndex = 0;
        currentScene = null;
    }

    public void OpenDialogue(int sceneID)
    {
        gameObject.SetActive(true); //open the dialogue screen

        currentScene = dialogueDB.GetQuestDialogueByID(sceneID); //get the scene that we clicked on

        UpdateDialogueText();
        SetupResponse();
    }

    private void UpdateDialogueText()
    {
        dialogueLineText.SetText(currentScene.GetLine(currentLineIndex).DialogueText); //display the current line of text in the scene
    }

    private void SetupResponse()
    {
        currentResponse = currentScene.GetLine(currentLineIndex).Response; //Get the response (next, complete, accept, etc) of this dialogue line
        rightButton.GetComponentInChildren<TextMeshProUGUI>().SetText(currentResponse.ResponseText);
    }

    public void ResponseSelected()
    {
        switch (currentResponse.TypeOfResponse)
        {
            case DialogueResponse.ResponseType.CONTINUE: ContinueDialogue();
                break;
            case DialogueResponse.ResponseType.ACCEPT: FinishDialogue();
                break;
            case DialogueResponse.ResponseType.END: FinishDialogue();
                break;
        }
    }

    public void ContinueDialogue()
    {
        currentLineIndex++;
        UpdateDialogueText();
        SetupResponse();
    }

    public void FinishDialogue()
    {
        CloseDialogue();
    }
}
