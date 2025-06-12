using UnityEngine;

public enum ItemType
{
    Weapon,
    Potion,
    Armor,
    Misc
}

public abstract class ItemInfo : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public bool isStackable;
    public int maxStack = 1;
    public int itemValue;
}