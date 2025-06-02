using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipEnable : Singelton<TooltipEnable>
{
    private static TooltipEnable instance;
    [SerializeField] private Tooltip tooltip;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }	

    public static void EnableTooltip()
    {
        instance.tooltip.gameObject.SetActive(true);
    }
    public static void DisableTooltip()
    {
        instance.tooltip.gameObject.SetActive(false);
    }
}
