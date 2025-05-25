using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateActiveWeapon : MonoBehaviour
{
    private void Awake()
    {

    }
    void Update()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            MouseFollow();
        }
    }
    
    public void MouseFollow()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - transform.position).normalized;
        //transform.right = direction; // alternative
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
