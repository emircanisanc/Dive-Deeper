using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsurperTheBoss : EnemyBaseAbstract
{

    [Header("USURPER")]
    [Header("Attack 1")]
    public float attack1DurationBeforeDamage = 0.11f;
    public float attack1DurationAfterDamage = 0.05f;
    public float attack1DurationRate = 5f;
    public string attack1AnimName = "Attack0";
    public float attack1Range;
    public Collider attack1Collider;
    float nextAttack1Time;

    [Header("Attack 1")]
    public float attack2DurationBeforeDamage = 0.11f;
    public float attack2DurationAfterDamage = 0.05f;
    public float attack2DurationRate = 5f;
    public string attack2AnimName = "Attack1";
    public float attack2Range;
    public Collider attack2Collider;
    float nextAttack2Time;

    public override void SetPlayer()
    {
        base.SetPlayer();
        animator.SetBool("IsWalking", true);
    }

    public override void UpdateMethod()
    {
        if (!canMove)
            return;
        if (!isAttacking)
        {
            playerPos = player.position;
            float distanceToPlayer = Vector3.Distance(transform.position, playerPos);
            agent.SetDestination(playerPos);
            if (distanceToPlayer <= attack1Range && Time.time >= nextAttack1Time)
            {
                FaceTarget();
                Attack();
            }
            else if (distanceToPlayer <= attack2Range && Time.time >= nextAttack2Time)
            {
                FaceTarget();
                Attack2();
            }

        }
    }


    public override void Attack()
    {
        StartCoroutine(Attack1Coroutine());
    }

    IEnumerator Attack1Coroutine()
    {
        isAttacking = true;
        agent.velocity = Vector3.zero;
        StopMove();
        transform.LookAt(player);
        animator.SetTrigger(attack1AnimName);
        yield return new WaitForSeconds(attack1DurationBeforeDamage);
        attack1Collider.enabled = true;
        yield return new WaitForSeconds(attack1DurationAfterDamage);
        attack1Collider.enabled = false;
        StartMove();
        isAttacking = false;
        nextAttack1Time = Time.time + attack1DurationRate;
    }

    public void Attack2()
    {
        StartCoroutine(Attack2Coroutine());
    }

    IEnumerator Attack2Coroutine()
    {
        isAttacking = true;
        agent.velocity = Vector3.zero;
        StopMove();
        transform.LookAt(player);
        animator.SetTrigger(attack2AnimName);
        yield return new WaitForSeconds(attack1DurationBeforeDamage);
        attack2Collider.enabled = true;
        yield return new WaitForSeconds(attack1DurationAfterDamage);
        attack2Collider.enabled = false;
        yield return new WaitForSeconds(1.5f);
        StartMove();
        isAttacking = false;
        nextAttack2Time = Time.time + attack2DurationRate;
    }





}
