using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    GameObject parentObj;
    [SerializeField] QuestGrid inProgressQuests;
    [SerializeField] QuestGrid completedQuests;
    [SerializeField] GameObject questGridPrefab;
    [SerializeField] GameObject inProgressParent;
    [SerializeField] GameObject completedParent;
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questReqLvl;
    [SerializeField] TextMeshProUGUI questDesc;
    [SerializeField] RequirementsGrid requirementsGrid;
    int selectedQuestID = -1;

    private void Awake()
    {
        parentObj = GameObject.FindGameObjectWithTag("QuestCanvas");
    }
    // Start is called before the first frame update
    void Start()
    {
        Show(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedQuestID != -1)
        {
            UpdateDescription(selectedQuestID);
        }
    }

    public void Show(bool maybe)
    {
        parentObj.SetActive(maybe);
        if (maybe)
        {
            ShowInProgress();
        }
    }

    public bool Showing()
    {
        return parentObj.activeInHierarchy;
    }

    public void ShowInProgress()
    {
        inProgressParent.gameObject.SetActive(true);
        completedParent.gameObject.SetActive(false);
    }

    public void ShowCompleted()
    {
        inProgressParent.gameObject.SetActive(false);
        completedParent.gameObject.SetActive(true);
    }

    public void UpdateDescription(int id, bool newSelection = false)
    {
        Quest selectedQuest = QuestDB.Instance.GetQuestByID(id);
        selectedQuestID = id;

        if(newSelection)
        {
            questName.SetText(selectedQuest.Title);
            questReqLvl.SetText(selectedQuest.LevelRequired.ToString());
            questDesc.SetText(selectedQuest.Description);

            requirementsGrid.NewGrid(selectedQuest);
        }
        else
        {
            requirementsGrid.UpdateCurrentGrid(selectedQuest);
        }
    }
    
    public void AddToGrid(int id)
    {
        Quest quest = QuestDB.Instance.GetQuestByID(id);

        if (!quest.QuestCompleted)
        {
            inProgressQuests.AddToGrid(questGridPrefab, quest);
        }
        else
        {
            inProgressQuests.RemoveFromGrid(quest.QuestID);
            completedQuests.AddToGrid(questGridPrefab, quest);
        }
    }

}
