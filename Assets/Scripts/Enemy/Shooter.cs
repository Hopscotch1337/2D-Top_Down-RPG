using System.Collections;
using UnityEngine;


public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private int burstCount = 4;
    [SerializeField] private int projectilesPerBurst = 1;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float spawningDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts = 0.3f;
    [SerializeField] private float restTimeAfterBurst = 1f;
    [SerializeField] private bool stagger;
    [Tooltip("If true, the shooter will oscillate between two angles when shooting.")]
    [SerializeField] private bool oscilate;
    private bool isShooting = false;
    private float timeBetweenBrustsOriginal;

    void OnValidate()
    {
        if (burstCount < 1) { burstCount = 1; }
        if (projectilesPerBurst < 1) { projectilesPerBurst = 1; }
        if (angleSpread == 0) { projectilesPerBurst = 1; }
        if (oscilate) { stagger = true; }
        if (!oscilate) { stagger = false; }
        if (spawningDistance < 0.1f) { spawningDistance = 0.1f; }
        if (timeBetweenBursts < 0.1f) { timeBetweenBursts = 0.1f; }
        if (projectileSpeed < 0.1f) { projectileSpeed = 0.1f; }
    }
    private void Awake()
    {
        timeBetweenBrustsOriginal = timeBetweenBursts;
    }

    public void Attack()
    {
        if (!isShooting) { StartCoroutine(ShootRoutine()); }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float startAngle, currentAngle, angleStepSize, endAngle;
        float timeBetweenProjectiles = 0f;
        if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst; }
        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStepSize, out endAngle);

        for (int i = 0; i < burstCount; i++)
        {
            if (!oscilate) { TargetConeOfInfluence(out startAngle, out currentAngle, out angleStepSize, out endAngle); }

            if (oscilate && (i % 2 != 1))
            {
                timeBetweenBursts = 0f;
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStepSize, out endAngle);
            }
            else if (oscilate)
            {
                timeBetweenBursts = timeBetweenBrustsOriginal;
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStepSize = -angleStepSize;
            }
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                currentAngle = startAngle + (j * angleStepSize);
                Vector2 pos = FindBulletSpawnPoint(currentAngle);

                GameObject newProjectile = Instantiate(projectilePrefab, pos, Quaternion.identity);
                newProjectile.transform.right = newProjectile.transform.position - transform.position;
                if (newProjectile.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateProjectileSpeed(projectileSpeed);
                    projectile.UpdateProjectileRange(projectileRange);
                }
                if (stagger)
                {
                    yield return new WaitForSeconds(timeBetweenProjectiles);
                }
            }
            currentAngle = startAngle;
            yield return new WaitForSeconds(timeBetweenBursts);
        }
        yield return new WaitForSeconds(restTimeAfterBurst);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStepSize, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        float angleStep = targetAngle;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = targetAngle;
        angleStepSize = targetAngle;
        if (angleSpread != 0)
        {
            halfAngleSpread = angleSpread / 2f;
            startAngle -= halfAngleSpread;
            endAngle += halfAngleSpread;
            angleStepSize = angleSpread / (projectilesPerBurst - 1);
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPoint(float currentAngle)
    {
        float x = transform.position.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * spawningDistance;
        float y = transform.position.y + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * spawningDistance;

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
