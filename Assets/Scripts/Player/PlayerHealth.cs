using System.Collections;

using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
   // [SerializeField] private GameObject deathEffect;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackTrust = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    private KnockBack knockBack;
    private SpriteFlash spriteFlash;
    private int currentHealth;
    private bool canTakeDamage = true;

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        spriteFlash = GetComponent<SpriteFlash>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI isEnemy = other.gameObject.GetComponent<EnemyAI>();
        if (isEnemy != null)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void TakeDamage(int damageamount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ScreenShake();
        knockBack.GetKnockedBack(hitTransform, knockBackTrust);
        StartCoroutine(DamageRecoveryCooldown());
        StartCoroutine(spriteFlash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageamount;

        if (currentHealth <= 0)
        {
            // Die();
        }
    }
        
    IEnumerator DamageRecoveryCooldown()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
    
}
