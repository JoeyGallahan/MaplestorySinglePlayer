using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSelectionUI : MonoBehaviour
{
    [SerializeField] GameObject dialogueOptionPrefab;
    GameObject gridView;
    NPCDialogueScenes dialogueScenes;

    public NPCDialogueScenes DialogueScenes
    {
        get => dialogueScenes;
        set
        {
            dialogueScenes = value;
        }
    }

    private void Awake()
    {
        gridView = GetComponentInChildren<ContentSizeFitter>().gameObject;
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
    public void OpenSelection(GameObject npc)
    {
        gameObject.SetActive(true);

        dialogueScenes = npc.GetComponent<NPCDialogueScenes>();
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        KillGrid();

        List<DialogueScene> scenes = dialogueScenes.scenes;

        for (int i = 0; i < scenes.Count; i++)
        {
            GameObject newObj;
            newObj = (GameObject)Instantiate(dialogueOptionPrefab, gridView.transform);

            TextMeshProUGUI dialogueTitle = newObj.GetComponentInChildren<TextMeshProUGUI>();
            dialogueTitle.SetText(scenes[i].Title);
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
