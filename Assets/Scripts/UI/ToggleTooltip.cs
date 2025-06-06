using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTooltip : Singelton<ToggleTooltip>
{
    private static ToggleTooltip instance;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private float FadeTooltipDuration = 0.1f;
    private CanvasGroup canvasGroup;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
        canvasGroup = tooltip.GetComponent<CanvasGroup>();
    }

    public static void EnableTooltip(string header, string content)
    {
        instance.canvasGroup.alpha = 0f;
        if (header == "" && content == "") { return; }
        instance.tooltip.SetText(header, content);
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.FadeInTooltip());
    }

    IEnumerator FadeInTooltip()
    {
        instance.tooltip.gameObject.SetActive(true);

        float timer = 0f;

        while (timer < FadeTooltipDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / FadeTooltipDuration);
            canvasGroup.alpha = t;  // 0 → 1
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }
    public static void DisableTooltip()
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.FadeoutTooltip());
    }
    IEnumerator FadeoutTooltip()
    {

        float timer = 0f;

        while (timer < FadeTooltipDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(1f - (timer / FadeTooltipDuration));
            canvasGroup.alpha = t;  // 1 -> 0
            yield return null;
        }
        canvasGroup.alpha = 0f;
        instance.tooltip.gameObject.SetActive(false);
    }
}
