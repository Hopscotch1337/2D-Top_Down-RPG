using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ShopUI : Singelton<ShopUI>
{
    [Header("UI-Container")]
    public GameObject window;
    public Transform  buyArea;
    public Transform  sellArea;
    public ShopSlot shopSlotPrefab;
    public TMP_Text   statusText;
    public TMP_Text   shopItemsHeader; // Überschrift "Shop-Items"
    [Range(0f,1f)] [SerializeField] private float sellMagnitude = 0.5f;

    private VendorData currentVendor;

    public void OpenShop(VendorData vendor)
    {
        currentVendor = vendor;
        window.SetActive(true);
        RefreshShop();
        // Only open inventory if it's currently closed
        if (!InventoryUI.Instance.gameObject.activeSelf)
        {
            InventoryUI.Instance.gameObject.SetActive(true);
        }
        statusText.gameObject.SetActive(false);
    }

    public void CloseShop()
    {
        window.SetActive(false);
    }

private void RefreshShop()
    {
        // 1) Alte Einträge weghauen
        foreach (Transform child in buyArea)
            Destroy(child.gameObject);

        // 2) Shop-Slots neu füllen
        foreach (var vi in currentVendor.stock)
        {
            var slot = Instantiate(shopSlotPrefab, buyArea);
            // statt vi.price verwenden wir jetzt vi.itemInfo.itemValue
            slot.Setup(vi.itemInfo, vi.itemInfo.itemValue);
            slot.SetQuantity(vi.quantity);
        }

        // 3) Optional: Inventar-UI im "Verkaufen"-Bereich updaten
        InventoryUI.Instance.RefreshAll();
    }

    // wird von Sell-Drop-Handler gerufen
    public int SellItem(SlotType slotType, int slotIndex) {
        Debug.Log("sell wurde ausgeführt");
        
        InventoryItem entry = null;
        if (slotType == SlotType.Inventory)
        {
            entry = InventoryManager.Instance.inventoryItems[slotIndex];
        }
        else if (slotType == SlotType.Hotbar)
        {
            entry = InventoryManager.Instance.hotbarItems[slotIndex];
        }
        
        if (entry == null) return 0;
        int sellPrice = (int)(entry.itemInfo.itemValue * sellMagnitude); 

        statusText.text = $"Sold {entry.quantity}  {entry.itemInfo.itemName} for {sellPrice * entry.quantity}G";
        statusText.gameObject.SetActive(true);

        // Add the sold item back to the shop's inventory
        currentVendor.AddToStock(entry.itemInfo, entry.quantity);

        EconomyManager.Instance.UpdateGoldCoins(sellPrice * entry.quantity);
        InventoryManager.Instance.RemoveFromList(slotType, slotIndex, entry.quantity);
        
        // Refresh shop to show the new stock
        RefreshShop();
        
        return sellPrice;
    }

    // wird von ShopSlotUI beim Kauf-Drop auf InventorySlot gerufen
    public void BuyItem(ItemInfo info, int targetSlot = -1)
    {

        // 1) Prüfen, ob genug Gold da ist
        if (EconomyManager.Instance.CurrentGoldAmount < info.itemValue)
        {
            statusText.text = "Not enough gold!";
            statusText.gameObject.SetActive(true);
            return;
        }

        // 2) Erst im Vendor-Stock abziehen (gestapelter Bestand!)
        bool removed = currentVendor.RemoveFromStock(info, 1);
        if (!removed){ return; }

        // 3) Versuche, genau in targetSlot zu legen
        bool ok = false;
        if (targetSlot >= 0)
        {
            ok = InventoryManager.Instance.PlaceOrStackAt(targetSlot, info, 1);
        }
        if (!ok)
        {
            // Fallback: erstes freies Inventar-Feld
            ok = InventoryManager.Instance.AddToInventory(info, 1);
        }

        if (ok)
        {
            // 4) Gold abbuchen
            EconomyManager.Instance.UpdateGoldCoins(-info.itemValue);

            // 5) UI updaten
            if (targetSlot >= 0) InventoryUI.Instance.UpdateSlot(targetSlot);
            RefreshShop(); // aktualisiere auch die Anzeige im Shop (Bestand)

            statusText.text = $"Bought {info.itemName} for {info.itemValue}G";
            statusText.gameObject.SetActive(true);
        }
        else
        {
            // stell das Item wieder in den Vendor-Stock zurück:
            currentVendor.AddToStock(info, 1);
            statusText.text = "Inventory full!";
            statusText.gameObject.SetActive(true);
        }
    }
}