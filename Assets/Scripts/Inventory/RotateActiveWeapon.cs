using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateActiveWeapon : MonoBehaviour
{
    private Transform weaponTransform;

    private void Awake()
    {
        weaponTransform = this.transform;
    }
    void Update()
    {

    }
    
    public void MouseFollow()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - this.gameObject.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
