using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemInfo itemInfo;  // Referenz auf dein ScriptableObject (WeaponInfo, PotionInfo…)
    public int quantity;       // Stapel­größe oder Anzahl

    public InventoryItem(ItemInfo info, int qty = 1)
    {
        itemInfo = info;
        quantity = qty;
    }
}