using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerContoller playerContoller;
    private ActiveWeapon activeWeapon;

    private void Awake() {
        playerContoller = GetComponentInParent<PlayerContoller>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        playerControls = new PlayerControls();
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start() {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update() {
        MouseFollowWithOffset();
    }

    private void Attack() {
        myAnimator.SetTrigger("Attack");
    }

    private void MouseFollowWithOffset(){
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerContoller.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        if (mousePos.x < playerScreenPoint.x){
            activeWeapon.transform.rotation = Quaternion.Euler (0, -180, angle);
        }
        else{
            activeWeapon.transform.rotation = Quaternion.Euler (0, 0, angle);
        }
        
        
    }
}
