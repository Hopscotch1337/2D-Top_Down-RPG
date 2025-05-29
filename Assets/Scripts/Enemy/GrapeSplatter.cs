
using UnityEngine;

public class GrapeSplatter : MonoBehaviour
{
    private SpriteFade spriteFade;
    [SerializeField] private float colliderStayTime = 0.3f;
    private void Awake()
    {
        spriteFade = GetComponent<SpriteFade>();
    }

    private void Start()
    {
        StartCoroutine(spriteFade.SpriteFadeOutRoutine());
        Invoke("DisableCollider", colliderStayTime);
    }
    private void DisableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1, transform);
        }

    }
    
    
}
