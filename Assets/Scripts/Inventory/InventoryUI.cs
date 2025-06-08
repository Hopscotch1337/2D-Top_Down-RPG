using UnityEngine;

public class InventoryUI : Singelton<InventoryUI>
{
    private InventorySlot[] slots;

    protected override void Awake()
    {
        base.Awake();
        // findet alle InventorySlot-Komponenten in den Children
        slots = GetComponentsInChildren<InventorySlot>();
    }


  
    private void OnEnable()
    {
        RefreshAll();
    }

    // Alle Slots neu zeichnen.
    public void RefreshAll()
    {
        if (slots==null || slots.Length == 0) slots = GetComponentsInChildren<InventorySlot>();
        foreach (var slot in slots)
            slot.Refresh();
    }

    /// Ein einzelnes Slot-Update (z.B. nach Verbrauch).
    public void UpdateSlot(int index)
    {
        if (slots==null || slots.Length == 0) slots = GetComponentsInChildren<InventorySlot>();
        if (index >= 0 && index < slots.Length)
            slots[index].Refresh();
    }



    // Optional: wenn du ganz klassisch mit Input.GetKeyDown arbeiten willst,
    // kannst du hier das B-Toggle einbauen:
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            gameObject.SetActive(!gameObject.activeSelf);
    }
    */
}