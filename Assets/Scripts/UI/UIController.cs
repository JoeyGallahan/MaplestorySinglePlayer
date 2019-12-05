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
    UIEffects uiEffects;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        playerCharacterUI = GameObject.FindGameObjectWithTag("CharacterCanvas").GetComponentInChildren<PlayerCharacterUI>();

        gainsUI = GameObject.FindGameObjectWithTag("GainsCanvas").GetComponent<GainsUI>();

        skillsUI = GameObject.FindGameObjectWithTag("SkillCanvas").GetComponentInChildren<SkillsUI>();

        inventoryUI = GameObject.FindGameObjectWithTag("InventoryCanvas").GetComponentInChildren<InventoryUI>();

        uiEffects = GameObject.FindGameObjectWithTag("EffectsCanvas").GetComponent<UIEffects>();
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

        //We only want to update the AP texts/changes if it's actually showing. Should save a bit on resources
        if (playerCharacterUI.Showing())
        {
            UpdateAPTexts();
            playerCharacterUI.ToggleAPChanges(true);
        }
    }

    //Fade out to a black screen
    public void FadeToBlack()
    {
        uiEffects.fadingOut = true;
    }

    //Fade in from a black screen
    public void FadeInFromBlack()
    {
        uiEffects.fadingIn = true;
    }

    //If you're fading out or not. Used for better timing when teleporting
    public bool FadingOut()
    {
        return uiEffects.fadingOut;
    }

    //Updates the UI texts related to your Ability Points and damage range
    public void UpdateAPTexts()
    {
        playerCharacterUI.UpdateTexts();
    }

    //Adds to the Gains UI
    public void AddGain(string amount, string type)
    {
        gainsUI.AddGain(amount, type);
    }

    //Shows and hides the character screen
    private void ToggleCharacterUI()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerCharacterUI.Show(!playerCharacterUI.Showing());
        }
    }
    
    //Shows and hides the skills screen
    private void ToggleSkillsUI()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            skillsUI.Show(!skillsUI.Showing());
        }
    }

    //Shows and hides your inventory
    private void ToggleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.Show(!inventoryUI.Showing());
        }
    }
}
