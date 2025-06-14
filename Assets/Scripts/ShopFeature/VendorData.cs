using System.Collections.Generic;
using UnityEngine;

public class VendorData : MonoBehaviour
{
    [Header("Welcher Bestand (gestapelt) verfügbar ist")]
    public List<VendorItem> stock = new List<VendorItem>();

    /// <summary>
    /// Fügt dem Vendor-Bestand ein Item hinzu (stackt gleiche Items).
    /// </summary>
    public void AddToStock(ItemInfo info, int amount = 1)
    {
        // Erst nach einem bestehenden Eintrag suchen
        Debug.Log("add stock" + info);
        var existing = stock.Find(x => x.itemInfo == info);
        if (existing != null)
        {
            existing.quantity += amount;
        }
        else
        {
            stock.Add(new VendorItem(info, amount));
        }
    }

    /// <summary>
    /// Entfernt eine bestimmte Menge aus dem Bestand. Gibt zurück, ob vollständig entfernt wurde.
    /// </summary>
    public bool RemoveFromStock(ItemInfo info, int amount = 1)
    {
        var existing = stock.Find(x => x.itemInfo == info);
        if (existing == null) return false;

        existing.quantity -= amount;
        if (existing.quantity <= 0)
            stock.Remove(existing);

        return true;
    }
}