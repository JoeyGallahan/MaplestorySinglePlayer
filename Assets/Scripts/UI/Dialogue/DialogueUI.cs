using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]Button rightButton;
    TextMeshProUGUI dialogueLineText;
    [SerializeField]DialogueScene currentScene;
    DialogueResponse currentResponse;
    [SerializeField]int currentLineIndex = 0;
    int currentQuestID = -1;

    private void Awake()
    {
        dialogueLineText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<TextMeshProUGUI>();
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
        currentQuestID = -1;
    }

    public void OpenDialogue(int questID)
    {
        gameObject.SetActive(true); //open the dialogue screen

        currentScene = QuestDB.Instance.GetQuestByID(questID).GetCurrentDialogueScene(); //get the scene that we clicked on
        currentQuestID = questID;
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
            case DialogueResponse.ResponseType.ACCEPT: AcceptQuest();
                break;
            case DialogueResponse.ResponseType.END: FinishDialogue();
                break;
            case DialogueResponse.ResponseType.COMPLETE: CompleteDialogue();
                break;
        }
    }

    private void ContinueDialogue()
    {
        currentLineIndex++;
        UpdateDialogueText();
        SetupResponse();
    }

    private void FinishDialogue()
    {
        CloseDialogue();
    }

    private void AcceptQuest()
    {
        PlayerCharacter.Instance.ActivateQuest(currentQuestID);
        QuestDB.Instance.GetQuestByID(currentQuestID).StartQuest();
        CloseDialogue();
    }

    private void CompleteDialogue()
    {
        QuestDB.Instance.GetQuestByID(currentQuestID).CompleteQuest();
        CloseDialogue();
    }
}
