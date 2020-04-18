using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSelectionUI : MonoBehaviour
{
    [SerializeField] GameObject dialogueOptionPrefab;
    GameObject gridView;
    NPCDialogueScenes dialogueList;
    QuestList questList;
    List<DialogueScene> scenes;

    public List<DialogueScene> DialogueScenes
    {
        get => scenes;
        set
        {
            scenes = value;
        }
    }

    private void Awake()
    {
        gridView = GetComponentInChildren<ContentSizeFitter>().gameObject;
        dialogueList = GameObject.FindGameObjectWithTag("GameController").GetComponent<NPCDialogueScenes>();
        questList = GameObject.FindGameObjectWithTag("GameController").GetComponent<QuestList>();
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

        scenes = dialogueList.GetScenesByNPCID(npc.NPCID); //Get all of their regular dialogue

        //Get all of their relevant quest dialogues
        foreach (DialogueScene questDiag in questList.GetQuestDialogueByNPCID(npc.NPCID))
        {
            scenes.Add(questDiag);
        }

        UpdateGrid();
    }

    private void UpdateGrid()
    {
        KillGrid();
        
        for (int i = 0; i < scenes.Count; i++)
        {
            GameObject newObj;
            newObj = (GameObject)Instantiate(dialogueOptionPrefab, gridView.transform);

            TextMeshProUGUI dialogueTitle = newObj.GetComponentInChildren<TextMeshProUGUI>();
            dialogueTitle.SetText(scenes[i].Title);

            newObj.GetComponent<DialogueSceneID>().SceneID = scenes[i].SceneID;
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
