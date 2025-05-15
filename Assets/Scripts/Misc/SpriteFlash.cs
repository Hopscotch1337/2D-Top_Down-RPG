using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float flashingTime = .2f;
    

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;
    private EnemyHealth enemyHealth;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth =GetComponent<EnemyHealth>();
        defaultMat = spriteRenderer.material;
    }

    public IEnumerator FlashRoutine() {
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(flashingTime);
        spriteRenderer.material = defaultMat;
        enemyHealth.DetectDeath();
    }
}
