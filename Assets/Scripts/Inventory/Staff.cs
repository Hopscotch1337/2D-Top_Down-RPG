using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    public void Attack()
    {
        // Implement the attack logic for the staff here
        Debug.Log("Staff attack!");
        ActiveWeapon.Instance.ToggleIsAttacking(false);

    }
}
