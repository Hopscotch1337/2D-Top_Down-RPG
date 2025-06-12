using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour ,IWeapon
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private WeaponInfo weaponInfo;

    private Transform weaponCollider;
    private Transform slashAnimationSpawnPoint;
    private Animator myAnimator;
    private GameObject slashAnimation;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        weaponCollider = PlayerController.Instance.GetWeaponcollider();
        slashAnimationSpawnPoint = PlayerController.Instance.GetSlashAnimationSpawnPoint();
    }

    private void Start() {
        weaponCollider.gameObject.SetActive(false);
    }
    private void Update()
    {
        MouseFollowWithOffset();
    }
public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
    
    public void Attack()
    {
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);
        slashAnimation = Instantiate(slashAnimationPrefab, slashAnimationSpawnPoint.position, Quaternion.identity);
        slashAnimation.transform.parent = this.transform.parent;
    }


    private void DoneAttackingAnimationEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimationEvent(){
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180,0,0);
        if (PlayerController.Instance.FacingLeft) {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }else{
            slashAnimation.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void SwingDownAnimationEvent(){
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        if (PlayerController.Instance.FacingLeft) {
            slashAnimation.GetComponent<SpriteRenderer>().flipX = true;
        }else{
            slashAnimation.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void MouseFollowWithOffset(){
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        if (mousePos.x < playerScreenPoint.x){
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler (0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler (0, -180, 0);
        }
        else{
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler (0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler (0, 0, 0);
        }
    }
}
