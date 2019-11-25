using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerCharacterUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    PlayerCharacter player;

    Vector3 offset;
    GameObject parentObj;
    GameObject expandParent;
    GameObject apChangesParent;

    //Equips
    EquipmentUI weaponEquip;

    TextMeshProUGUI characterName;
    TextMeshProUGUI remainingAP;
    TextMeshProUGUI strAmountText, dexAmountText, intAmountText, lukAmountText;
    TextMeshProUGUI dmgRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();

        parentObj = GameObject.FindGameObjectWithTag("CharacterCanvas");
        expandParent = GameObject.FindGameObjectWithTag("CharacterCanvasExpanded");
        apChangesParent = GameObject.FindGameObjectWithTag("APChanges");

        characterName = GameObject.FindGameObjectWithTag("CharacterInfoName").GetComponent<TextMeshProUGUI>();

        remainingAP = GameObject.FindGameObjectWithTag("RemainingAP").GetComponent<TextMeshProUGUI>();

        dmgRange = GameObject.FindGameObjectWithTag("DamageRange").GetComponent<TextMeshProUGUI>();

        strAmountText = GameObject.FindGameObjectWithTag("StrAmount").GetComponent<TextMeshProUGUI>();
        dexAmountText = GameObject.FindGameObjectWithTag("DexAmount").GetComponent<TextMeshProUGUI>();
        intAmountText = GameObject.FindGameObjectWithTag("IntAmount").GetComponent<TextMeshProUGUI>();
        lukAmountText = GameObject.FindGameObjectWithTag("LukAmount").GetComponent<TextMeshProUGUI>();

        weaponEquip = GameObject.FindGameObjectWithTag("EquipWeapon").GetComponent<EquipmentUI>();
    }

    private void Start()
    {
        Show(false);
        expandParent.SetActive(false);
        ToggleAPChanges(false);
    }

    public void Show(bool maybe)
    {
        parentObj.SetActive(maybe);
    }
    public bool Showing()
    {
        return parentObj.activeInHierarchy;
    }

    public void Expand()
    {
        expandParent.SetActive(!expandParent.activeInHierarchy);

        if (expandParent.activeInHierarchy)
        {
            UpdateTexts();
        }
    }

    public void UpdateTexts()
    {
        characterName.SetText(player.PlayerName);

        remainingAP.SetText(player.RemainingApPoints.ToString());

        string dmgText = player.BaseDamage.ToString() + " - " + player.MaxDamage.ToString();

        dmgRange.SetText(dmgText);

        strAmountText.SetText(player.apSTR.ToString());
        dexAmountText.SetText(player.apDEX.ToString());
        intAmountText.SetText(player.apINT.ToString());
        lukAmountText.SetText(player.apLUK.ToString());
    }

    public void SaveAP()
    {
        ToggleAPChanges(false);
        player.SaveAP();
        UpdateTexts();
    }

    public void CancelAP()
    {
        player.CancelAP();
        UpdateTexts();
    }

    public void ToggleAPChanges(bool maybe)
    {
        apChangesParent.SetActive(maybe);
    }

    public void AddEquip(string type, int id)
    {
        switch (type)
        {
            case "Weapon": weaponEquip.AddEquip(id);
                break;
        }

        UpdateTexts();
    }

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset.x = Input.mousePosition.x - transform.position.x;
        offset.y = Input.mousePosition.y - transform.position.y;
    }
    #endregion

    #region IDragHandler implementation
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition - offset;
    }
    #endregion

    #region IEndDragHandler implementation
    public void OnEndDrag(PointerEventData eventData)
    {
        offset = Vector3.zero;
    }
    #endregion
}
