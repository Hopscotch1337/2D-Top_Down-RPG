using UnityEngine;

[System.Serializable]
public class VendorItem
{
    public ItemInfo itemInfo;
    public int      quantity;

    public VendorItem(ItemInfo info, int qty = 1)
    {
        itemInfo = info;
        quantity = qty;
    }
}