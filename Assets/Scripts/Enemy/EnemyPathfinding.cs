using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private  Vector2 roamingPosition;
    private Rigidbody2D myRigidbody;
    private KnockBack knockBack;

    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }
    private void FixedUpdate() {
        if (knockBack.gettingKnockedBack){return;}
        EnemyMove();
    }

    private void EnemyMove(){
            myRigidbody.MovePosition(myRigidbody.position + roamingPosition * (moveSpeed * Time.fixedDeltaTime));
    } 
    
    public void SetNewposition (Vector2 newPosition){
        roamingPosition = newPosition;
    }
}
