using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singelton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls;
    private bool isAttacking, attackButtonDown = false;


    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
        // weaponCollider.gameObject.SetActive(false);
    }
    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
    }
    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }


    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void StartAttacking()
    {
        attackButtonDown = true;
    }
    private void StopAttacking()
    {
        attackButtonDown = false;
    }
    private void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            // Call the attack method of the weapon
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
    
}
