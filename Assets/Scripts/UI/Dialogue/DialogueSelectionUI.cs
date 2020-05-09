using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSelectionUI : MonoBehaviour
{
    [SerializeField] GameObject dialogueOptionPrefab;
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

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseSelection()
    {
        gameObject.SetActive(false);
    }
    public void OpenSelection(NpcCharacter npc)
    {
        gameObject.SetActive(true);
        
        //Get all of their relevant quest dialogues
        quests = questDB.GetQuestsByNPCID(npc.NPCID);

        foreach (Quest q in quests)
        {
            if (q.QuestCompleted)
            {
                quests.Remove(q);
            }
        }

        UpdateGrid();
    }

    private void UpdateGrid()
    {
        KillGrid();
        
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
