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

    [HideInInspector] public List<InventoryItem> inventoryItems;
    [HideInInspector] public List<InventoryItem> hotbarItems;

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

        var tmp = listA[aIndex];
        listA[aIndex] = listB[bIndex];
        listB[bIndex] = tmp;

        // UIs updaten
        if (aType == SlotType.Inventory) InventoryUI.Instance.UpdateSlot(aIndex);
        else ActiveInventory.Instance.RefreshSlot(aIndex);

        if (bType == SlotType.Inventory) InventoryUI.Instance.UpdateSlot(bIndex);
        else ActiveInventory.Instance.RefreshSlot(bIndex);

            // Nur wenn einer der getauschten Slots aktuell aktiv ist:
        int active = ActiveInventory.Instance.ActiveInventoryIndex;
        if ((aType == SlotType.Hotbar && aIndex == active) ||(bType == SlotType.Hotbar && bIndex == active))
        {
            ActiveInventory.Instance.RefreshActiveSlot();
        }
    }
    

}