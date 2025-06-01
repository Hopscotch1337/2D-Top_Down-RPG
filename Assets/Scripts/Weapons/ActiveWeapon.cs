using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singelton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls;
    private bool isAttacking, attackButtonDown = false;
    private float weaponCooldown;
    


    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
        HandleWeaponCooldwon();
    }
    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        weaponCooldown = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().attackCooldown;
        HandleWeaponCooldwon();
    }
        public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    // private void OnDisable()
    // {
    //     playerControls.Disable();
    // }

    private void HandleWeaponCooldwon()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(CooldownRoutine());
    }
    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(weaponCooldown);
        isAttacking = false;
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
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon is IWeapon)
        {
            (CurrentActiveWeapon as IWeapon).Attack();
            HandleWeaponCooldwon();
        }
    }
    
}
