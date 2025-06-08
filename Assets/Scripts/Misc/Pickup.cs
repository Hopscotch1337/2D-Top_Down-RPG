using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pickup : MonoBehaviour
{

    private enum PickupType
    {
        GoldCoin,
        HealthGlobe,
        StaminaGlobe
    }
    [SerializeField] private PickupType pickupType;
    [SerializeField] private float pickupDistance = 4f;
    [SerializeField] private float startingMoveSpeed = 1f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private AnimationCurve spawnCurve;
    [SerializeField] private float rangeFromSpawn = 1f;
    [SerializeField] private float flightDuration = 0.3f;
    [SerializeField] private float curveHeight = 1.5f;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = startingMoveSpeed;
    }
    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update()
    {
        CheckDistance();
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            DetectPickupType();
            Destroy(gameObject);
        }
    }

    private void CheckDistance()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < pickupDistance)
        {
            direction = (PlayerController.Instance.transform.position - transform.position).normalized;
            moveSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            direction = Vector2.zero;
            moveSpeed = startingMoveSpeed;
        }
    }
    private void MoveToPlayer()
    {
        rb.velocity = direction * moveSpeed * Time.deltaTime;
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * rangeFromSpawn + startPosition;

        float elapsedTime = 0f;

        while (elapsedTime < flightDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flightDuration; // Normalize time to [0, 1]

            Vector3 linearPos = Vector3.Lerp(startPosition, endPosition, t);
            float heightOffset = spawnCurve.Evaluate(t) * curveHeight;
            transform.position = new Vector3(linearPos.x, linearPos.y + heightOffset, linearPos.z);

            yield return null;
        }
    }

    private void DetectPickupType()
    {
        switch (pickupType)
        {
            case PickupType.GoldCoin:
                EconomyManager.Instance.UpdateGoldCoins(1);
                break;
            case PickupType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer(1);
                break;
            case PickupType.StaminaGlobe:
                Stamina.Instance.RestoreStamina(1);
                break;
            default:
                break;
        }
    }
}
