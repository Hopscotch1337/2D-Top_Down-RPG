using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private GameObject impactEffect; 
    private WeaponInfo weaponInfo;
    private Vector3 projectileStartPoint;

    void Start()
    {
        projectileStartPoint = transform.position;
    }

    void Update()
    {
        ProjectileMovement();
        CheckProjectileRange();
    }
    public void Initialize(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
    }

    private void ProjectileMovement()
    {
        transform.Translate(Vector3.right * projectileSpeed * Time.deltaTime);
    }
    private void CheckProjectileRange()
    {
        if (Vector3.Distance(transform.position, projectileStartPoint) > weaponInfo.weaponRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructable indestructable = other.GetComponent<Indestructable>();

        if (!other.isTrigger && (enemyHealth || indestructable))
        {
            enemyHealth?.TakeDamage(weaponInfo.weaponDamage);
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
}
