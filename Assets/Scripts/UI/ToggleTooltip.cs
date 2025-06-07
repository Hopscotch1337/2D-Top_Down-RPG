using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleTooltip : Singelton<ToggleTooltip>
{
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private float FadeTooltipDuration = 0.1f;
    private CanvasGroup canvasGroup;
    private EventSystem eventSystem;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = tooltip.GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        canvasGroup.alpha = 0f;
        tooltip.gameObject.SetActive(false);
    }

    public void EnableTooltip(string header, string content)
    {
        canvasGroup.alpha = 0f;
        if (header == "" && content == "") { return; }
        tooltip.SetText(header, content);
        StopAllCoroutines();
        StartCoroutine(FadeInTooltip());
    }

    IEnumerator FadeInTooltip()
    {
        tooltip.gameObject.SetActive(true);
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
    public void DisableTooltip()
    {
        StopAllCoroutines();
        StartCoroutine(FadeoutTooltip());
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
        tooltip.gameObject.SetActive(false);
    }
}
