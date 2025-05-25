using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField]private float laserMaxGrowTime = 4f; // Time to reach full range
    private bool isGrowing = true;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    private WeaponInfo weaponInfo;
    private float laserRange;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }
    public void Initialize(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
        UpdateLaserRange();
    }
    public void UpdateLaserRange()
    {
        laserRange = weaponInfo.weaponRange;
        StartCoroutine(IncreaseLaserRangeRoutine());
    }

    private IEnumerator IncreaseLaserRangeRoutine()
    {
        float timePassed = 0f;
        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float fullSizeTime = timePassed / laserMaxGrowTime;
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, fullSizeTime), 1f);

            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, fullSizeTime), spriteRenderer.size.y / 2f);
            capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1f, laserRange, fullSizeTime)) / 2f, 0f); // Adjust offset to keep the collider centered

            yield return null;
        }
        StartCoroutine(GetComponent<SpriteFade>().SpriteFadeOutRoutine());
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Indestructable>() && !other.isTrigger) 
        {
            isGrowing = false;
        }
        
    }
    
    private void LaserFaceMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        //transform.right = direction; // alternative
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
