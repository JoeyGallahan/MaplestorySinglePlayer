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

    TextMeshProUGUI characterName;
    TextMeshProUGUI strAmountText, dexAmountText, intAmountText, lukAmountText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();

        parentObj = GameObject.FindGameObjectWithTag("CharacterCanvas");
        expandParent = GameObject.FindGameObjectWithTag("CharacterCanvasExpanded");

        characterName = GameObject.FindGameObjectWithTag("CharacterInfoName").GetComponent<TextMeshProUGUI>();

        strAmountText = GameObject.FindGameObjectWithTag("StrAmount").GetComponent<TextMeshProUGUI>();
        dexAmountText = GameObject.FindGameObjectWithTag("DexAmount").GetComponent<TextMeshProUGUI>();
        intAmountText = GameObject.FindGameObjectWithTag("IntAmount").GetComponent<TextMeshProUGUI>();
        lukAmountText = GameObject.FindGameObjectWithTag("LukAmount").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Show(false);
        expandParent.SetActive(false);
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

    private void UpdateTexts()
    {
        characterName.SetText(player.PlayerName);

        strAmountText.SetText(player.apSTR.ToString());
        dexAmountText.SetText(player.apDEX.ToString());
        intAmountText.SetText(player.apINT.ToString());
        lukAmountText.SetText(player.apLUK.ToString());
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
