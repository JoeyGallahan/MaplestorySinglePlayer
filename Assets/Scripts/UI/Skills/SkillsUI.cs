using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    GameObject parentObj;
    SkillDB skillDB;
    SkillsGrid starterSkills;
    [SerializeField] GameObject skillGridPrefab;
    [SerializeField] GameObject skillDescriptionPrefab;
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
}
