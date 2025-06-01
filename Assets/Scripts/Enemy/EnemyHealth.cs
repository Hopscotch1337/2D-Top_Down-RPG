using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackForce = 15f;
    

    private int currentHealth;
    private KnockBack knockBack;
    private SpriteFlash spriteFlash;


    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        spriteFlash = GetComponent<SpriteFlash>();
    }
    private void Start() {
        currentHealth = startingHealth;
    }
    public void TakeDamage (int damage){
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackForce); 
        StartCoroutine(spriteFlash.FlashRoutine());
    }

    public void DetectDeath(){
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickupSpawner>().DropItems();
            Destroy(gameObject);
        }
    }
}
