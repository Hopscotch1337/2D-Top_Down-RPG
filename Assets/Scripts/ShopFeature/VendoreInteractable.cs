using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class VendoreInteractable : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    private OutlineToggle outlineToggle;

    private bool playerNearby = false;

    private void Awake() {
        outlineToggle = GetComponent<OutlineToggle>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
        
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            outlineToggle.SetOutlineEnabled(true);

            OnPlayerEnter?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            outlineToggle.SetOutlineEnabled(false);
            ShopUI.Instance.CloseShop();
            OnPlayerExit?.Invoke();
        }
    }

    private void OnMouseDown()
    {
        // wenn Pointer über irgendeinem UI-Element ist, nichts tun
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;
        if (!playerNearby) return;
        var data = GetComponent<VendorData>();
        if (data != null) ShopUI.Instance.OpenShop(data);
    }
}