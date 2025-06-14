using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class SellAreaDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var invSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();
        if (invSlot == null || invSlot.slotType != SlotType.Inventory) return;

        // Verkauf ausführen
        ShopUI.Instance.SellItem(invSlot.slotIndex);

    }
}