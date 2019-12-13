using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SkillsUI : MonoBehaviour
{
    GameObject parentObj;
    SkillDB skillDB;
    SkillsGrid starterSkills;
    [SerializeField] GameObject skillGridPrefab;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] Image skillImage;
    [SerializeField] TextMeshProUGUI skillReqLvl;
    [SerializeField] TextMeshProUGUI skillMP;
    [SerializeField] TextMeshProUGUI skillDesc;

    PlayerCharacter player;

    private void Awake()
    {
        parentObj = GameObject.FindGameObjectWithTag("SkillCanvas");
        skillDB = GameObject.FindGameObjectWithTag("GameController").GetComponent<SkillDB>();
        starterSkills = parentObj.GetComponentInChildren<SkillsGrid>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }
    // Start is called before the first frame update
    void Start()
    {
        starterSkills.AddToGrid(skillGridPrefab, skillDB.GetSkillsByClass(player.ClassName));
        UpdateDescription(0);
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
        Skill selectedSkill = skillDB.GetSkillByID(id);

        skillName.SetText(selectedSkill.skillName);
        skillReqLvl.SetText(selectedSkill.levelRequired.ToString());
        skillMP.SetText(selectedSkill.mpUsed.ToString());
        skillDesc.SetText(selectedSkill.description);

        skillImage.sprite = selectedSkill.skillSprite.GetComponent<Image>().sprite;
    }
}
