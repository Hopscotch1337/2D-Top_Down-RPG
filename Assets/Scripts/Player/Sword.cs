using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimationSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerContoller playerContoller;
    private ActiveWeapon activeWeapon;
    private bool isAttacking, attackButtonDown = false;

    private GameObject slashAnimation;

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

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
        weaponCollider.gameObject.SetActive(false);
    }

    private void Update()
    {
        MouseFollowWithOffset();
        StartCoroutine(AttackCDRoutine());
    }

    private void StartAttacking(){
        attackButtonDown = true;
    }
    private void StopAttacking(){
        attackButtonDown = false;
    }

    private IEnumerator AttackCDRoutine(){
        if (attackButtonDown &! isAttacking)
        {
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnPoint.position, Quaternion.identity);
            slashAnimation.transform.parent = this.transform.parent;
            yield return new WaitForSeconds(attackCooldown);
            isAttacking = false;
        }
    }

    private void DoneAttackingAnimationEvent(){

        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimationEvent(){
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);
        if (playerContoller.FacingLeft) {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }else{
            slashAnimation.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void SwingDownAnimationEvent(){
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        if (playerContoller.FacingLeft) {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }else{
            slashAnimation.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void MouseFollowWithOffset(){
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerContoller.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        if (mousePos.x < playerScreenPoint.x){
            activeWeapon.transform.rotation = Quaternion.Euler (0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler (0, -180, 0);
        }
        else{
            activeWeapon.transform.rotation = Quaternion.Euler (0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler (0, 0, 0);
        }
    }
}
