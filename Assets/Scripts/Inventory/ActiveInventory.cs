using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : Singelton<ActiveInventory>

{
    private int activeInventoryIndex = 0;
    private PlayerControls playerControls;



    protected override void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ChangeActiveInventory((int)ctx.ReadValue<float>());
        ChangeActiveInventory(1);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void ChangeActiveInventory(int keyboardValue)
    {
        HighlightActiveInventory(keyboardValue - 1);
    }

    private void HighlightActiveInventory(int index)
    {
        activeInventoryIndex = index;
        foreach (Transform inventorySlot in transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(activeInventoryIndex).GetChild(0).gameObject.SetActive(true);
        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }
        if (!transform.GetChild(activeInventoryIndex).GetComponent<InventorySlot>())
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }
        
            GameObject weaponToSpawn = transform.GetChild(activeInventoryIndex).GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;
            GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);

            newWeapon.transform.parent = ActiveWeapon.Instance.transform;
            ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
        
    }
        
}
