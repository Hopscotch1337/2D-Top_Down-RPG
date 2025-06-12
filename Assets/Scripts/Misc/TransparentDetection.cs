using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class TransparentDetection : MonoBehaviour
{
    [Header("Transparency Settings")]
    [Range(0,1)] [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = 0.5f;

    [Header("Extras")]
    [Tooltip("Soll die Transparenz-Einstellung auch auf alle Child-SpriteRenderers und Tilemaps angewendet werden?")]
    [SerializeField] private bool includeChildren = false;

    private SpriteRenderer spriteRenderer;
    private Tilemap       tilemap;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap        = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null) return;
        // Ziel-Alpha setzen
        float target = transparencyAmount;
        // Fade starten für alle relevanten Renderer
        StartFade(target);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() == null) return;
        // Ziel-Alpha wieder 1
        StartFade(1f);
    }

    private void StartFade(float targetAlpha)
    {
        // SpriteRenderer(s)
        if (includeChildren)
        {
            foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
                StartCoroutine(FadeRoutine(sr, fadeTime, sr.color.a, targetAlpha));
        }
        else if (spriteRenderer != null)
        {
            StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, targetAlpha));
        }

        // Tilemap(s)
        if (includeChildren)
        {
            foreach (var tm in GetComponentsInChildren<Tilemap>())
                StartCoroutine(FadeRoutine(tm, fadeTime, tm.color.a, targetAlpha));
        }
        else if (tilemap != null)
        {
            StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, targetAlpha));
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer sr, float duration, float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(start, end, elapsed / duration);
            var c = sr.color; c.a = a;
            sr.color = c;
            yield return null;
        }
    }

    private IEnumerator FadeRoutine(Tilemap tm, float duration, float start, float end)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(start, end, elapsed / duration);
            var c = tm.color; c.a = a;
            tm.color = c;
            yield return null;
        }
    }
}