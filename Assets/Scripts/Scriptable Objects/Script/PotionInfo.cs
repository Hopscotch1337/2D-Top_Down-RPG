// PotionInfo.cs
using UnityEngine;
using System.Collections.Generic;

public enum PotionEffectType
{
    Heal,
    MoveSpeed,
    DamageIncrease,
    Stamina,
    Invulnerable

}

[System.Serializable]
public class PotionEffect
{
    public PotionEffectType effectType;
    public float magnitude;     // z.B. Heal-Amount oder Prozent-Wert für Buffs
    public float duration;      // Dauer in Sekunden (für Instant-Effekte: 0)
}

[CreateAssetMenu(menuName = "Inventory/New Potion")]
public class PotionInfo : ItemInfo
{
    [Tooltip("Liste aller Effekte dieses Tranks")]
    public List<PotionEffect> effects = new List<PotionEffect>();

    private void OnValidate()
    {
        itemType = ItemType.Potion;
        isStackable = true;
    }
}