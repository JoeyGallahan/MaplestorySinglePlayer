using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    PlayerCharacter player;
    PlayerCharacterUI playerCharacterUI;
    GainsUI gainsUI;
    SkillsUI skillsUI;
    InventoryUI inventoryUI;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        playerCharacterUI = GameObject.FindGameObjectWithTag("CharacterCanvas").GetComponentInChildren<PlayerCharacterUI>();

        gainsUI = GameObject.FindGameObjectWithTag("GainsCanvas").GetComponent<GainsUI>();

        skillsUI = GameObject.FindGameObjectWithTag("SkillCanvas").GetComponentInChildren<SkillsUI>();

        inventoryUI = GameObject.FindGameObjectWithTag("InventoryCanvas").GetComponentInChildren<InventoryUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ToggleCharacterUI();
        ToggleSkillsUI();
        ToggleInventory();

        if (playerCharacterUI.Showing())
        {
            UpdateAPTexts();
            playerCharacterUI.ToggleAPChanges(true);
        }
    }

    public void UpdateAPTexts()
    {
        playerCharacterUI.UpdateTexts();
    }

    public void AddGain(string amount, string type)
    {
        gainsUI.AddGain(amount, type);
    }

    private void ToggleCharacterUI()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerCharacterUI.Show(!playerCharacterUI.Showing());
        }
    }
    
    private void ToggleSkillsUI()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            skillsUI.Show(!skillsUI.Showing());
        }
    }

    private void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.Show(!inventoryUI.Showing());
        }
    }
}
