
using UnityEngine;
using Unity.Mathematics;


public class ActiveInventory : Singelton<ActiveInventory>
{
    public InventorySlot[] hotbarSlots; // Inspector: deine 5 Slots
    private PlayerControls controls;
    public int ActiveInventoryIndex { get; private set; } = 0;

    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method
        
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Inventory.Hotbar.performed += ctx => ChangeActiveSlot((int)ctx.ReadValue<float>() - 1);
        controls.Inventory.OpenInventory.performed += _ => ToggleInventory();
        controls.Inventory.Use.performed += _ => UseActiveItem();
    }

    private void Start()
    {
        if (hotbarSlots == null || hotbarSlots.Length == 0) hotbarSlots = GetComponentsInChildren<InventorySlot>();
    }

    public void RefreshSlot(int idx)
    {
        if (hotbarSlots == null || hotbarSlots.Length == 0) hotbarSlots = GetComponentsInChildren<InventorySlot>();
        hotbarSlots[idx].Refresh();
    }
    public void RefreshActiveSlot()
    {
        ChangeActiveSlot(ActiveInventoryIndex);
    }
    public void ChangeActiveSlot(int idx)
    {
        if (hotbarSlots == null || hotbarSlots.Length == 0) hotbarSlots = GetComponentsInChildren<InventorySlot>();
        ActiveInventoryIndex = Mathf.Clamp(idx, 0, hotbarSlots.Length - 1);

        // Highlight
        for (int i = 0; i < hotbarSlots.Length; i++)
            hotbarSlots[i].SetHighlight(i == ActiveInventoryIndex);

        // Equip-Logik
        EquipWeapon();

        // synchronisieren mit Inventar-Popup (optional)
        // InventoryUI.Instance.Highlight(activeIndex);
    }

    private void EquipWeapon()
    {
        Debug.Log("equip weapon wurde ausgeführt");
        if (PlayerHealth.Instance.IsDead) return;

        if (ActiveWeapon.Instance.CurrentActiveWeapon != null) Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);

        var entry = InventoryManager.Instance.hotbarItems[ActiveInventoryIndex];
        if (entry?.itemInfo is WeaponInfo wInfo)
        {
            var spawnedWeapon = Instantiate(wInfo.weaponPrefab, ActiveWeapon.Instance.transform.position, Quaternion.identity); //,ActiveWeapon.Instance.transform
            ActiveWeapon.Instance.transform.rotation = quaternion.Euler(0, 0, 0); //Richtet die Waffe nach 000 aus

            spawnedWeapon.transform.parent = ActiveWeapon.Instance.transform; //setzt die Waffe an das Transform des Parent
            ActiveWeapon.Instance.NewWeapon(spawnedWeapon.GetComponent<MonoBehaviour>());
        }
        else
        {
            Debug.Log("keine waffe");
            ActiveWeapon.Instance.WeaponNull();
        }
    }

    private void UseActiveItem()
    {
        var entry = InventoryManager.Instance.hotbarItems[ActiveInventoryIndex];
        if (entry == null) return;

        if (entry.itemInfo is PotionInfo pInfo)
        {
            PotionManager.Instance.UsePotion(pInfo);
            InventoryManager.Instance.RemoveFromList(SlotType.Hotbar, ActiveInventoryIndex, 1);
            RefreshSlot(ActiveInventoryIndex);
        }
    }

    private void ToggleInventory()
    {
        if (InventoryUI.Instance.gameObject.activeSelf)
        {
            InventoryUI.Instance.gameObject.SetActive(false);
        }
        else
        {
            InventoryUI.Instance.gameObject.SetActive(true);
        }
    }

}