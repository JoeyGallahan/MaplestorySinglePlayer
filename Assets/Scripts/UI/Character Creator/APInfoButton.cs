using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class APInfoButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject infoScreen;

    private void Start()
    {
        infoScreen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoScreen.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoScreen.SetActive(false);
    }
}
