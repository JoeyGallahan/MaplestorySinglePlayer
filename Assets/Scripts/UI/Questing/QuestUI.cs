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
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questReqLvl;
    [SerializeField] TextMeshProUGUI questDesc;
    [SerializeField] RequirementsGrid requirementsGrid;

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
    }

    public void Show(bool maybe)
    {
        parentObj.SetActive(maybe);
    }

    public bool Showing()
    {
        return parentObj.activeInHierarchy;
    }

    public void UpdateDescription(int id)
    {
        Quest selectedQuest = QuestDB.Instance.GetQuestByID(id);

        questName.SetText(selectedQuest.Title);
        questReqLvl.SetText(selectedQuest.LevelRequired.ToString());
        questDesc.SetText(selectedQuest.Description);

        requirementsGrid.UpdateGrid(selectedQuest);
    }
    
    public void AddToGrid(int id)
    {
        Quest quest = QuestDB.Instance.GetQuestByID(id);

        if (!quest.QuestCompleted)
        {
            inProgressQuests.AddToGrid(questGridPrefab, quest);
        }

    }

}
