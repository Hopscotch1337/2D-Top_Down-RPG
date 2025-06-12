using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class InventorySlot : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;   // Inspector: Inventory oder Hotbar
    public int      slotIndex;  // Inspector: Index in der jeweiligen Liste

    [Header("UI-Referenzen")]
    [SerializeField] private Image     iconImage;
    [SerializeField] private GameObject highlightBorder;
    [SerializeField] private TMP_Text quantityText;

    private CanvasGroup canvasGroup;
    private Canvas      rootCanvas;
    private GameObject  dragIcon;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // finde das oberste Canvas, damit unser Drag-Icon dort eingehängt wird
        rootCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
            // 1) Manager & Liste da?
        if (InventoryManager.Instance == null) { iconImage.enabled = false; return; }
        var list = slotType == SlotType.Inventory
            ? InventoryManager.Instance.inventoryItems
            : InventoryManager.Instance.hotbarItems;

        // 2) Index im gültigen Bereich?
        if (slotIndex < 0 || slotIndex >= list.Count) { iconImage.enabled = false; return; }

        // 3) Entry holen
        var entry = list[slotIndex];

        // 4) Wenn kein Item oder kein Icon: Icon verbergen und Sprite löschen
        if (entry == null || entry.itemInfo == null || entry.itemInfo.icon == null)
        {
            iconImage.enabled = false;
            iconImage.sprite  = null;
            quantityText.text = "";
            return;
        }

        // 5) Ansonsten Icon setzen
        iconImage.enabled = true;
        iconImage.sprite  = entry.itemInfo.icon;
        quantityText.text = entry.itemInfo.isStackable ? "x" + entry.quantity.ToString(): "";
    }

    public void SetHighlight(bool on)
    {
        if (highlightBorder != null)
            highlightBorder.SetActive(on);
    }

    #region Drag & Drop
    public void OnBeginDrag(PointerEventData evt)
    {
        // nur ziehen, wenn wirklich ein Icon da ist
        if (!iconImage.enabled || InventoryManager.Instance == null) 
            return;

        // 1) neues Drag-Icon anlegen
        dragIcon = new GameObject("DragIcon");
        var img = dragIcon.AddComponent<Image>();
        img.sprite      = iconImage.sprite;
        img.raycastTarget = false;

        // 2) Größe und Parent übernehmen
        var rt = dragIcon.GetComponent<RectTransform>();
        rt.SetParent(rootCanvas.transform, false);
        rt.sizeDelta = iconImage.rectTransform.sizeDelta;

        // 3) starte direkt an Zeigeposition
        dragIcon.transform.position = evt.position;

        // 4) Original-Slot durchlässig machen (für Drop)
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData evt)
    {
        if (dragIcon != null)
            dragIcon.transform.position = evt.position;
    }

    public void OnEndDrag(PointerEventData evt)
    {
        // wieder blocken und Drag-Icon entfernen
        canvasGroup.blocksRaycasts = true;
        if (dragIcon != null)Destroy(dragIcon);

    }

    public void OnDrop(PointerEventData evt)
    {
        var shopSlot = evt.pointerDrag?.GetComponent<ShopSlot>();
        Debug.Log($"OnDrop auf Slot #{slotIndex} vom Typ {slotType}");


        if (shopSlot != null && slotType == SlotType.Inventory)
        {
            // hier kaufen, also nichts tun
            ShopUI.Instance.BuyItem(shopSlot.itemInfo, slotIndex);
            shopSlot.CleanupDragIcon();
            return;
        }
        var other = evt.pointerDrag?.GetComponent<InventorySlot>();
        if (other != null)
        {
            // Swap Slots
            InventoryManager.Instance.SwapSlots(slotType, slotIndex, other.slotType, other.slotIndex);
            return;
        }
    }
    #endregion



    #region Toggle Tooltip
    public void OnPointerEnter(PointerEventData evt)
    {
        var entry = slotType == SlotType.Inventory
            ? InventoryManager.Instance.inventoryItems[slotIndex]
            : InventoryManager.Instance.hotbarItems[slotIndex];

        if (entry != null && entry.quantity > 1)
            ToggleTooltip.Instance.EnableTooltip("", entry.itemInfo.itemName + " x " + entry.quantity);
        else if (entry != null && entry.quantity == 1)
        {
            ToggleTooltip.Instance.EnableTooltip("", entry.itemInfo.itemName);
        }
    }

    public void OnPointerExit(PointerEventData evt) => ToggleTooltip.Instance.DisableTooltip();
    #endregion
}