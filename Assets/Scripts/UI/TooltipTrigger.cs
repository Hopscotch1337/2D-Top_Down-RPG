using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // This method is called when the pointer enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipEnable.EnableTooltip();
    }

    // This method is called when the pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipEnable.DisableTooltip();
    }
}

