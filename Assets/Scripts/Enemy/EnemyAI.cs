using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirectionTime = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType; // This should be a script that implements IEnemy interface
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool attackWhileMoving = true;

    private enum State
    {
        Roaming,
        Attacking,
    }
    private bool canAttack = true;
    private float roamTimer = 0f;
    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Vector2 newRoamPosition;



    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        newRoamPosition = GetRandomRoamingPosition();
    }
    
    private void Update()
    {
        MovmentControl();
    }

    private void MovmentControl()
    {
        switch (state)
        {
            case State.Roaming:
                Roaming();
                break;
            case State.Attacking:
                Attacking();
                break;
            default: break;
        }
    }

    private void Roaming()
    {
        roamTimer += Time.deltaTime;
        enemyPathfinding.SetNewposition(newRoamPosition);
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackRange)
        {
            state = State.Attacking;
            return;
        }
        if (roamTimer >= roamChangeDirectionTime)
        {
            roamTimer = 0f;
            newRoamPosition = GetRandomRoamingPosition();
        }
        
    }
    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) >= (attackRange + 2f))
        {
            state = State.Roaming;
            return;
        }
        
        if (attackWhileMoving == false)
        {
            enemyPathfinding.CancelMovement();
        }
        else
        {
            enemyPathfinding.SetNewposition((PlayerController.Instance.transform.position - transform.position).normalized * 1.6f);
        }
        
        if (canAttack && attackRange != 0)
        {
            canAttack = false;

            (enemyType as IEnemy).Attack();
            StartCoroutine(AttackCooldownRoutine());
        }

    }
    IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown); // Adjust the cooldown time as needed
        canAttack = true;
    }
    private Vector2 GetRandomRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

    }

}
