using System.Collections;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private GameObject splashPrefab;
    [SerializeField] private float flightDuration = 2f;
    [SerializeField] private float curveHeight = 2f;
    [SerializeField] private AnimationCurve animCurve;

    private void OnValidate()
{
    if (animCurve == null)
        animCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);
}

    private void Start()
    {
        StartCoroutine(ProjectileCurveRoutine(transform.position, PlayerController.Instance.transform.position));
        StartCoroutine(ShadowMoveRoutine(transform.position, PlayerController.Instance.transform.position));
    }


    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float elapsedTime = 0f;

        while (elapsedTime < flightDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flightDuration; // Normalize time to [0, 1]

            Vector3 linearPos = Vector3.Lerp(startPosition, endPosition, t);
            float heightOffset = animCurve.Evaluate(t) * curveHeight;
            transform.position = new Vector3(linearPos.x, linearPos.y + heightOffset, linearPos.z);

            yield return null;
        }
        Instantiate(splashPrefab, endPosition, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator ShadowMoveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        GameObject shadow = Instantiate(shadowPrefab, startPosition, Quaternion.identity);
        float elapsedTime = 0f;

        while (elapsedTime < flightDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flightDuration;

            shadow.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        Destroy(shadow);
    }
}
