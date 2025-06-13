using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Daten")]
    public ItemInfo itemInfo;
    public int      price;
    private int     quantity;

    [Header("UI-References")]
    public Image    iconImage;
    public TMP_Text priceText;
    public TMP_Text quantityText;

    private GameObject dragIcon;
    private Canvas    rootCanvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rootCanvas  = GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// Initialisierung des Slots
    /// </summary>
    public void Setup(ItemInfo info, int cost, int qty = 1)
    {
        itemInfo      = info;
        price         = cost;
        quantity      = qty;

        iconImage.sprite = info.icon;
        iconImage.enabled = info.icon != null;

        priceText.text   = cost.ToString();
        SetQuantity(qty);
    }

    /// <summary>
    /// Doppelklick zum Kaufen
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2 && itemInfo != null)
        {
            // Doppelklick - Item kaufen
            ShopUI.Instance.BuyItem(itemInfo);
        }
    }

    /// <summary>
    /// Zeige oder verstecke die Stückzahl
    /// </summary>
    public void SetQuantity(int qty)
    {
        quantity = qty;
        if (quantity > 1)
        {
            quantityText.gameObject.SetActive(true);
            quantityText.text = "x" + quantity.ToString();
        }
        else
        {
            quantityText.gameObject.SetActive(false);
        }
    }

    #region Drag & Drop
    public void OnBeginDrag(PointerEventData e)
    {
        if (!iconImage.enabled) return;

        // Drag-Icon erzeugen
        dragIcon = new GameObject("ShopDragIcon");
        var img  = dragIcon.AddComponent<Image>();
        img.sprite       = iconImage.sprite;
        img.raycastTarget = false;

        // Größe + Parent übernehmen
        var rt = dragIcon.GetComponent<RectTransform>();
        rt.SetParent(rootCanvas.transform, false);
        rt.sizeDelta = iconImage.rectTransform.sizeDelta;
        dragIcon.transform.position = e.position;

        // Original unterdrückt Raycasts, damit OnDrop auf InventorySlot feuert
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData e)
    {
        if (dragIcon != null)
            dragIcon.transform.position = e.position;
    }

    public void OnEndDrag(PointerEventData e)
    {
        CleanupDragIcon();
    }
    public void CleanupDragIcon()
    {
        if (dragIcon != null)
        {
            Destroy(dragIcon);
            dragIcon = null;
        }
        canvasGroup.blocksRaycasts = true;
    }
    #endregion

    #region Tooltip
    public void OnPointerEnter(PointerEventData e)
    {
        if (itemInfo == null) return;
        string title = itemInfo.itemName;
        string content = "Preis: " + price + "  ";
        if (quantity > 1) content += "x" + quantity;
        ToggleTooltip.Instance.EnableTooltip(title, content);
    }

    public void OnPointerExit(PointerEventData e)
    {
        ToggleTooltip.Instance.DisableTooltip();
    }
    #endregion
}