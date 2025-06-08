using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/New Potion")]
public class PotionInfo : ItemInfo
{
    public int healAmount;

    private void OnValidate()
    {
        itemType = ItemType.Potion;
        isStackable = true;
    }
}
