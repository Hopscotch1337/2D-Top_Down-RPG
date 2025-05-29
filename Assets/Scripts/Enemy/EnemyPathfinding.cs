using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Vector2 roamingPosition;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer spriteRenderer;
    private KnockBack knockBack;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myRigidbody = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }
    private void FixedUpdate()
    {
        if (knockBack.GettingKnockedBack) { return; }
        EnemyMove();
    }

    private void EnemyMove()
    {
        myRigidbody.MovePosition(myRigidbody.position + roamingPosition * (moveSpeed * Time.fixedDeltaTime));
        if (roamingPosition.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (roamingPosition.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }



    public void SetNewposition(Vector2 newPosition)
    {
        roamingPosition = newPosition;
    }
    public void CancelMovement()
    {
        roamingPosition = Vector2.zero;
    }
}
