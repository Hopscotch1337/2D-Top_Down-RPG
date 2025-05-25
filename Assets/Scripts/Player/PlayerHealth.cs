using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackTrust = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    private int currentHealth;
    [SerializeField] private GameObject deathEffect;
    private KnockBack knockBack;
    private SpriteFlash spriteFlash;
    private int damage = 1;
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
        if (isEnemy != null && canTakeDamage)
        {
            TakeDamage();
            StartCoroutine(spriteFlash.FlashRoutine());
            knockBack.GetKnockedBack(isEnemy.transform, knockBackTrust);
        }
    }


    public void TakeDamage()
    {
        canTakeDamage = false;
        currentHealth -= damage;
        
        StartCoroutine(DamageRecoveryCooldown());
        Debug.Log("Player took damage, current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            // Die();
        }
        
        IEnumerator DamageRecoveryCooldown()
        {
            yield return new WaitForSeconds(damageRecoveryTime);
            canTakeDamage = true;
        }
    }
}
