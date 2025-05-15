using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    

    private int currentHealth;
    private KnockBack knockBack;
    private SpriteFlash spriteFlash;

    private void Awake() {
        knockBack = GetComponent<KnockBack>();
        spriteFlash = GetComponent<SpriteFlash>();
    }
    private void Start() {
        currentHealth = startingHealth;
    }
    public void TakeDamage (int damage){
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerContoller.Instance.transform, 15f); 
        StartCoroutine(spriteFlash.FlashRoutine());
    }

    public void DetectDeath(){
        if (currentHealth <= 0){
            Destroy(gameObject);
        }
    }
}
