using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float projectileSpeed =20f; // Default speed, can be updated by the weapon
    [SerializeField] private float projectileRange = 10f; // Default range, can be updated by the weapon
    [SerializeField] private bool isEnemyProjectile = false;

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
    public void UpdateProjectileRange(float weaponRange)
    {
        this.projectileRange = weaponRange;
    }
    public void UpdateProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    private void ProjectileMovement()
    {
        transform.Translate(Vector3.right * projectileSpeed * Time.deltaTime);
    }
    private void CheckProjectileRange()
    {
        if (Vector3.Distance(transform.position, projectileStartPoint) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        Indestructable indestructable = other.GetComponent<Indestructable>();
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (!other.isTrigger && (enemyHealth || indestructable || player))
        {
            if ((isEnemyProjectile && player) || (!isEnemyProjectile && enemyHealth))
            {
                player?.TakeDamage(1, transform);
                enemyHealth?.TakeDamage(1);
                Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (indestructable)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            

        }

    }
}
