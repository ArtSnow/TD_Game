using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class UIPointer : MonoBehaviour, IPointerEnterHandler
{
    private string title;
    private Sprite sprite;
    private string description;
    private int type;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HUDStats.hUDStats.SetupHUD(title, sprite, description, type);
    }

    public void SetupInfo(string inTitle, Sprite inSprite, string inDescription, int inType)
    {
        title = inTitle;
        sprite = inSprite;
        description = inDescription;
        type = inType;
    }

}
