using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meele : MonoBehaviour, IEnemy
{
    [SerializeField] private Animator myAnimator;
    [Header("Idle Speed")]

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);
    }
}
