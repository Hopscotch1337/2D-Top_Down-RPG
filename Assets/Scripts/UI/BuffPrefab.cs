using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BuffPrefab : MonoBehaviour
{
    [Tooltip("Timer-Text (TMP)")][SerializeField] private TMP_Text timerText;
    [Tooltip("Icon-Image")][SerializeField] private Image iconImage;
    [Tooltip("Hintergrund, der von Grün zu Rot übergeht")][SerializeField] private Image backgroundImage;

    private float totalDuration;

    /// <summary>Initialisiert Icon und startet Countdown.</summary>
    public void Init(Sprite icon, float duration)
    {
        iconImage.sprite = icon;
        totalDuration = duration;
        StartCoroutine(CountdownAndDestroy(duration));
    }

    private IEnumerator CountdownAndDestroy(float duration)
    {
        float remainingTime = duration;
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();

            float t = Mathf.Clamp01(remainingTime / totalDuration);
            // t=1 → Grün, t=0 → Rot
            backgroundImage.color = Color.Lerp(Color.red, Color.green, t);

            yield return null;
        }
        Destroy(gameObject);

        
    }
    
}