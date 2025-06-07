using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string header;
    [SerializeField] private string content;


    // This method is called when the pointer enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleTooltip.Instance.EnableTooltip(header, content);
    }

    // This method is called when the pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        ToggleTooltip.Instance.DisableTooltip();
    }

    private void OnMouseEnter()
    {
        ToggleTooltip.Instance.EnableTooltip(header, content);
    }

    private void OnMouseExit()
    {
        ToggleTooltip.Instance.DisableTooltip();
    }
}

