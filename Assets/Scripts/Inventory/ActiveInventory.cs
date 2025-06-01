using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : Singelton<ActiveInventory>

{
    private int activeInventoryIndex = 0;
    private PlayerControls playerControls;



    protected override void Awake()
    {
        base.Awake();
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

    public void ChangeActiveInventory(int keyboardValue)
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
        if (PlayerHealth.Instance.IsDead){ return; }
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }
        Transform childTransform = transform.GetChild(activeInventoryIndex);
        InventorySlot inventorySlot = childTransform.GetComponent<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }
        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        // GameObject weaponToSpawn = transform.GetChild(activeInventoryIndex).GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;


        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
            ActiveWeapon.Instance.transform.rotation = quaternion.Euler(0, 0, 0);

            newWeapon.transform.parent = ActiveWeapon.Instance.transform;
            ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
        
}
