using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/New Weapon")]
public class WeaponInfo : ItemInfo
{
    public GameObject weaponPrefab;
    public float attackCooldown;
    public int weaponDamage;
    public float weaponRange;

    private void OnValidate()
    {
        itemType = ItemType.Weapon;
        isStackable = false;
        maxStack = 1;
    }
}