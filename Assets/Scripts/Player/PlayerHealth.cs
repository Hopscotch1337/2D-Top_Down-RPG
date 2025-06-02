using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singelton<PlayerHealth>
{
    // [SerializeField] private GameObject deathEffect;
    public bool IsDead { get; private set; }
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackTrust = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    private Slider healthSlider;
    private KnockBack knockBack;
    private SpriteFlash spriteFlash;
    private int currentHealth;
    private bool canTakeDamage = true;
    const string HEALTH_SLIDER = "HealthSlider";
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    private Animator myAnimator;
    [TextArea] string description = "PlayerHealth manages the player's health, damage, and death. It handles taking damage from enemies, healing, and updating the health slider UI. It also manages knockback effects when taking damage.";
    


    protected override void Awake()
    {
        base.Awake();

        knockBack = GetComponent<KnockBack>();
        spriteFlash = GetComponent<SpriteFlash>();

    }
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
        Stamina.Instance.ResetStaminaOnDeath();
        IsDead = false;

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
        UpdateHealthSlider();

        if (currentHealth <= 0 && !IsDead)
        {
            currentHealth = 0;
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator DeathRoutine()
    {
        GetComponent<Animator>().SetTrigger(DEATH_HASH);
        IsDead = true;
        Destroy(ActiveWeapon.Instance.gameObject);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        SceneManager.LoadScene("Town_0");
    }
    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateHealthSlider();
        }
    }

    IEnumerator DamageRecoveryCooldown()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
    
    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER).GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    
}
