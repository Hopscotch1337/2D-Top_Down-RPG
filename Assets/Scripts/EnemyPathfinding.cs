using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private  Vector2 roamingPosition;
    private Rigidbody2D myRigidbody;

    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate() {
        EnemyMove();
    }

    public void SetNewposition (Vector2 newPosition){
        roamingPosition = newPosition;
    }

    private void EnemyMove(){
            myRigidbody.MovePosition(myRigidbody.position + roamingPosition * (moveSpeed * Time.fixedDeltaTime));
    } 
}
