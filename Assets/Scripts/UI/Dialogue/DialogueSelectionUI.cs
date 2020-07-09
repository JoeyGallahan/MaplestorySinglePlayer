using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSelectionUI : MonoBehaviour
{
    [SerializeField] GameObject dialogueOptionPrefab;
    [SerializeField] TextMeshProUGUI dialogueNPCText;
    GameObject gridView;
    QuestDB questDB;
    [SerializeField] List<Quest> quests = new List<Quest>();
    
    private void Awake()
    {
        gridView = GetComponentInChildren<ContentSizeFitter>().gameObject;
        questDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<QuestDB>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseSelection();
    }
    
    public void CloseSelection()
    {
        KillGrid();
        quests = null;
        gameObject.SetActive(false);
    }
    public void OpenSelection(int npcID)
    {
        //Get all of their relevant quest dialogues
        quests = questDB.GetQuestsByNPCID(npcID);
        dialogueNPCText.SetText(NPCDB.Instance.GetNpcName(npcID));

        UpdateGrid();
        gameObject.SetActive(true);
    }

    private void UpdateGrid()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            GameObject newObj;
            newObj = (GameObject)Instantiate(dialogueOptionPrefab, gridView.transform);

            TextMeshProUGUI dialogueTitle = newObj.GetComponentInChildren<TextMeshProUGUI>();
            dialogueTitle.SetText(quests[i].Title);

            newObj.GetComponent<QuestID>().ID = quests[i].QuestID;
        }
    }

    private void KillGrid()
    {
        foreach(Transform child in gridView.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
