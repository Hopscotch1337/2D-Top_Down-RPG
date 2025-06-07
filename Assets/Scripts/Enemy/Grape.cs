
using Unity.Mathematics;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject grapeProjectilePrefab;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");


    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);
        
        if (transform.position.x < PlayerController.Instance.transform.position.x)
            { mySpriteRenderer.flipX = false;}
        else
            { mySpriteRenderer.flipX = true;}
    }
    
    public void InstanciateProjectileAnimEvent()
    {
        GameObject newGrape = Instantiate(grapeProjectilePrefab, transform.position, quaternion.identity);

    }
}
