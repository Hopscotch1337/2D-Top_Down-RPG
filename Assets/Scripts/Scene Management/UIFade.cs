using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singelton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeDuration = 1f;
    private IEnumerator fadeRoutine;
    public void FadeToBlack()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = FadeRoutine(1f);
        StartCoroutine(fadeRoutine);
    }
    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = FadeRoutine(0f);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha)) {
            float newAlpha = Mathf.MoveTowards(fadeScreen.color.a, targetAlpha, Time.deltaTime / fadeDuration);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, newAlpha);
            yield return null;
        }
    }
}
