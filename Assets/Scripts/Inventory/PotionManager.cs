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
                case PotionEffectType.DamageIncrease:
                case PotionEffectType.Stamina:
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
                // playerStats.MoveSpeed += effect.magnitude;
                Debug.Log($"Speed +{effect.magnitude} for {effect.duration}s");
                break;
            case PotionEffectType.DamageIncrease:
                // playerStats.DamageMultiplier += effect.magnitude;
                Debug.Log($"Damage +{effect.magnitude} for {effect.duration}s");
                break;
            // case PotionEffectType.Stamina:
            //     playerStats.StaminaRegen += effect.magnitude;
            //     Debug.Log($"Stamina Regen +{effect.magnitude} for {effect.duration}s");
            //     break;
        }

        // Warte Buff-Dauer
        yield return new WaitForSeconds(effect.duration);

        // Buff zurücksetzen
        switch (effect.effectType)
        {
            case PotionEffectType.MoveSpeed:
                // playerStats.MoveSpeed -= effect.magnitude;
                break;
            case PotionEffectType.DamageIncrease:
                // playerStats.DamageMultiplier -= effect.magnitude;
                break;
            // case PotionEffectType.Stamina:
            //     playerStats.StaminaRegen -= effect.magnitude;
            //     break;
        }
        Debug.Log($"{effect.effectType} buff ended.");
    }
}