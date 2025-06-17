using System.Collections;
using UnityEngine;

public class PotionManager : Singelton<PotionManager>
{
    [Header("UI-Referenzen")]
    [Tooltip("Parent-Transform mit Vertical Layout Group für Buff-Indikatoren")]
    public Transform buffIndicatorContainer;
    [Tooltip("Prefab für einen einzelnen Buff-Indikator (mit TMP-Text und Image)")]
    public GameObject buffIndicatorPrefab;
     [HideInInspector] public bool staminaBuffIsActive = false;

    /// <summary>
    /// Wird aufgerufen, wenn der Spieler einen Trank benutzt.
    /// </summary>
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
                case PotionEffectType.Invulnerable:
                    StartCoroutine(ApplyBuff(effect, potion.icon));
                    break;
            }
        }
    }

    private IEnumerator ApplyBuff(PotionEffect effect, Sprite icon)
    {

        // 1. Buff aktivieren
        switch (effect.effectType)
        {
            case PotionEffectType.MoveSpeed:
                PlayerController.Instance.IncreaseMovespeed(effect.magnitude);
                break;
            case PotionEffectType.DamageIncrease:
                // TODO: implementiere Damage-Multiplikator in deinem PlayerStats-Script
                Debug.Log($"Damage +{effect.magnitude} für {effect.duration}s");
                break;
            case PotionEffectType.Invulnerable:
                PlayerHealth.Instance.canTakeDamage = false;
                break;
            case PotionEffectType.Stamina:
                // Beispiel: setze Flag
                staminaBuffIsActive = true;
                break;
        }

                // ◼ 2) UI-Element instanziieren
        if (buffIndicatorPrefab == null || buffIndicatorContainer == null)
        {
            Debug.LogError("PotionManager: UI-Prefab oder Container nicht gesetzt!");
            yield break;
        }

        var prefab = Instantiate(buffIndicatorPrefab, buffIndicatorContainer);
        var buffPrefab = prefab.GetComponent<BuffPrefab>();
        if (buffPrefab == null)
        {
            Debug.LogError("PotionManager: Prefab hat kein BuffIndicatorUI-Script!");
            Destroy(prefab);
            yield break;
        }

        // ◼ 3) Initialisiere Icon + Countdown
        buffPrefab.Init(icon, effect.duration);

        // ◼ 4) Warte auf Ende des Buffs
        yield return new WaitForSeconds(effect.duration);

        // 5. Buff-Effekt rückgängig machen
        switch (effect.effectType)
        {
            case PotionEffectType.MoveSpeed:
                PlayerController.Instance.SetDefaultMovespeed();
                break;
            case PotionEffectType.DamageIncrease:
                // TODO: entferne Damage-Multiplikator
                break;
            case PotionEffectType.Invulnerable:
                PlayerHealth.Instance.canTakeDamage = true;
                break;
            case PotionEffectType.Stamina:
                staminaBuffIsActive = false;
                break;
        }

        Debug.Log($"{effect.effectType}-Buff abgelaufen.");
    }
}