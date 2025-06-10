// Potions.cs
using System.Collections;
using UnityEngine;

public class PotionManager : Singelton<PotionManager>
{
    //[Header("Referenzen")]


    /// <summary>
    /// Wird aufgerufen, wenn der Spieler einen Trank benutzt.
    /// </summary>
    /// 
    [HideInInspector] public bool staminaBuffIsActive = false;
     
    public void UsePotion(PotionInfo potion)
    {
        foreach (var effect in potion.effects)
        {
            switch (effect.effectType)
            {
                case PotionEffectType.Heal:
                    PlayerHealth.Instance.HealPlayer((int)effect.magnitude);
                    break;
                case PotionEffectType.MoveSpeed:
                    StartCoroutine(ApplyBuff(effect));
                    break;
                case PotionEffectType.DamageIncrease:
                    StartCoroutine(ApplyBuff(effect));
                    break;
                case PotionEffectType.Stamina:
                    StartCoroutine(ApplyBuff(effect));
                    break;
                case PotionEffectType.Invulnerable:
                    StartCoroutine(ApplyBuff(effect));
                    break;
            }
        }
    }



    private IEnumerator ApplyBuff(PotionEffect effect)
    {
        // Setze Buff
        switch (effect.effectType)
        {
            case PotionEffectType.MoveSpeed:
                PlayerController.Instance.IncreaseMovespeed(effect.magnitude);
                break;
            case PotionEffectType.DamageIncrease:
                // playerStats.DamageMultiplier += effect.magnitude;
                Debug.Log($"Damage +{effect.magnitude} for {effect.duration}s");
                break;
            case PotionEffectType.Invulnerable:
                PlayerHealth.Instance.canTakeDamage = false;
                break;
            case PotionEffectType.Stamina:
                staminaBuffIsActive = true;
                break;
        }

        // Warte Buff-Dauer
        yield return new WaitForSeconds(effect.duration);

        // Buff zurücksetzen
        switch (effect.effectType)
        {
            case PotionEffectType.MoveSpeed:
                PlayerController.Instance.SetDefaultMovespeed();
                break;
            case PotionEffectType.DamageIncrease:
                // playerStats.DamageMultiplier -= effect.magnitude;
                break;
            case PotionEffectType.Invulnerable:
                PlayerHealth.Instance.canTakeDamage = true;
                break;
            case PotionEffectType.Stamina:
                staminaBuffIsActive = false;
                break;
        }
        Debug.Log($"{effect.effectType} buff ended.");
    }
}