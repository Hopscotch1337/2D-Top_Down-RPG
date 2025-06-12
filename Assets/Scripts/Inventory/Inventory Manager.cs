using System.Collections.Generic;
using UnityEngine;

public enum SlotType { Inventory, Hotbar }

public class InventoryManager : Singelton<InventoryManager>
{
    //public static InventoryManager Instance { get; private set; }

    [Header("Anzahl Inventar-Slots")]
    public int inventorySlotCount = 12;
    [Header("Anzahl Hotbar-Slots")]
    public int hotbarSlotCount = 5;

    [Header("Später wieder im Script verstecken!!!")]
    public List<InventoryItem> inventoryItems;
    public List<InventoryItem> hotbarItems;

    [Header("Start-Items fürs Inventar")]
    public ItemInfo[] startingInventory;
    [Header("Start-Items für die Hotbar")]
    public ItemInfo[] startingHotbar;




    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method

        // Inventar initialisieren
        inventoryItems = new List<InventoryItem>(inventorySlotCount);
        for (int i = 0; i < inventorySlotCount; i++)
            inventoryItems.Add(null);

        // Hotbar initialisieren
        hotbarItems = new List<InventoryItem>(hotbarSlotCount);
        for (int i = 0; i < hotbarSlotCount; i++)
            hotbarItems.Add(null);
    }

    private void Start()
    {
        // fülle Inventar
        foreach (var info in startingInventory)
            AddToInventory(info, 1);

        // fülle Hotbar
        foreach (var info in startingHotbar)
            AddToHotbar(info, 1);

        InventoryUI.Instance.gameObject.SetActive(false);
        ActiveInventory.Instance.ChangeActiveSlot(0); // 0 is default Sword
    }

    // Fügt im Inventar hinzu
    public bool AddToInventory(ItemInfo info, int amount = 1)
    {
        return AddItemToList(inventoryItems, inventorySlotCount, info, amount,
            i => InventoryUI.Instance.UpdateSlot(i));
    }

    // Fügt in der Hotbar hinzu
    public bool AddToHotbar(ItemInfo info, int amount = 1)
    {
        return AddItemToList(hotbarItems, hotbarSlotCount, info, amount,
            i => ActiveInventory.Instance.RefreshSlot(i));
    }

    // Allgemeine Logik für Stapeln/Leeren Slot
    private bool AddItemToList(List<InventoryItem> list, int slotCount, ItemInfo info, int amount, System.Action<int> onSlotChanged)
    {
        // Stapeln
        if (info.isStackable)
        {
            for (int i = 0; i < slotCount; i++)
            {
                var slot = list[i];
                if (slot != null && slot.itemInfo == info && slot.quantity < info.maxStack)
                {
                    int space = info.maxStack - slot.quantity;
                    int toAdd = Mathf.Min(space, amount);
                    slot.quantity += toAdd;
                    amount -= toAdd;
                    onSlotChanged(i);
                    if (amount <= 0) return true;
                }
            }
        }

        // Leerer Slot
        while (amount > 0)
        {
            int free = list.FindIndex(x => x == null);
            if (free < 0) return false;
            int stack = info.isStackable ? Mathf.Min(amount, info.maxStack) : 1;
            list[free] = new InventoryItem(info, stack);
            amount -= stack;
            onSlotChanged(free);
        }
        return true;
    }

    // Entfernt Items
    public void RemoveFromList(SlotType type, int index, int amount = 1)
    {
        var list = type == SlotType.Inventory ? inventoryItems : hotbarItems;
        var onChanged = type == SlotType.Inventory
            ? new System.Action<int>(InventoryUI.Instance.UpdateSlot)
            : new System.Action<int>(ActiveInventory.Instance.RefreshSlot);

        var slot = list[index];
        if (slot == null) return;
        slot.quantity -= amount;
        if (slot.quantity <= 0) list[index] = null;
        onChanged(index);

        // Nur wenn Hotbar betroffen und es der aktive Slot war:
        if (type == SlotType.Hotbar && index == ActiveInventory.Instance.ActiveInventoryIndex)
            ActiveInventory.Instance.RefreshActiveSlot();
    }

    // Tauscht Slots innerhalb gleicher Liste oder zwischen Inventar/Hotbar
    public void SwapSlots(SlotType aType, int aIndex, SlotType bType, int bIndex)
    {
        var listA = aType == SlotType.Inventory ? inventoryItems : hotbarItems;
        var listB = bType == SlotType.Inventory ? inventoryItems : hotbarItems;

        var slotA = listA[aIndex];
        var slotB = listB[bIndex];

        // 1. Nur mergen, wenn beide Slots belegt sind, das gleiche Item und stackable:
        if (slotA != null && slotB != null && slotA.itemInfo == slotB.itemInfo && slotA.itemInfo.isStackable)
        {
            int combined = slotA.quantity + slotB.quantity;
            int maxStack = slotA.itemInfo.maxStack;

            if (combined <= maxStack)
            {
                // Alles passt in Slot B, A wird leer
                slotA.quantity = combined;
                slotB.itemInfo = null;
                slotB.quantity = 0;
            }
            else
            {
                // Slot B voll, verbleibender Rest in A
                slotA.quantity = maxStack;
                slotB.quantity = combined - maxStack;
            }

        }
        else
        {
            // Normales Swappen, wenn nicht mergen
            var tmp = listA[aIndex];
            listA[aIndex] = listB[bIndex];
            listB[bIndex] = tmp;
        }

        // UIs updaten
        if (aType == SlotType.Inventory) InventoryUI.Instance.UpdateSlot(aIndex);
        else ActiveInventory.Instance.RefreshSlot(aIndex);

        if (bType == SlotType.Inventory) InventoryUI.Instance.UpdateSlot(bIndex);
        else ActiveInventory.Instance.RefreshSlot(bIndex);

        // Nur wenn einer der getauschten Slots aktuell die aktive Hotbar ist:
        int active = ActiveInventory.Instance.ActiveInventoryIndex;
        if ((aType == SlotType.Hotbar && aIndex == active) || (bType == SlotType.Hotbar && bIndex == active))
        {
            ActiveInventory.Instance.RefreshActiveSlot();
        }
    }
    public bool PlaceItemAt(int slot, ItemInfo info, int amount = 1) //wird später evt. wieder benötigt für Frei und nichtfrei checks
    {
        if (slot < 0 || slot >= inventoryItems.Count)
            return false;

        if (inventoryItems[slot] != null)
            return false; // schon belegt

        inventoryItems[slot] = new InventoryItem(info, amount);
        return true;
    }
    
    public bool PlaceOrStackAt(int slotIndex, ItemInfo info, int amount = 1)
{
    // 1) Index-Check
    if (slotIndex < 0 || slotIndex >= inventoryItems.Count)
        return false;

    var slot = inventoryItems[slotIndex];

    // 2) Slot leer → normalen Place
    if (slot == null)
    {
        int toPlace = info.isStackable
            ? Mathf.Min(amount, info.maxStack)
            : 1;
        inventoryItems[slotIndex] = new InventoryItem(info, toPlace);
        return true;
    }

    // 3) Slot belegt, prüfen auf Stackbarkeit
    if (info.isStackable && slot.itemInfo == info && slot.quantity < info.maxStack)
    {
        int space = info.maxStack - slot.quantity;
        int toAdd = Mathf.Min(amount, space);
        slot.quantity += toAdd;
        return true;
    }

    // 4) Nichts ging → false
    return false;
}
    

}