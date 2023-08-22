using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : EnemyBaseAbstract
{
    private bool isRunning;
    private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float healthToStartRun = 30;

    protected override void Awake()
    {
        base.Awake();
        walkSpeed = moveSpeed;
    }

    public override void UpdateMethod()
    {
        base.UpdateMethod();
        if(!isRunning && currentHealth < healthToStartRun && !isDead)
        {
            isRunning = true;
            moveSpeed = runSpeed;
            agent.speed = moveSpeed;
            animator.SetFloat("MoveSpeed", 4f);
        }
    }

    protected override void Die()
    {
        coll.enabled = false;
        isDead = true;
        isAttacking = false;
        AnEnemyDied?.Invoke();
        StopAllCoroutines();
        StopChasing();
        animator.SetTrigger("Die");
        Invoke("Disappear", 2f);
        isRunning = false;
        moveSpeed = walkSpeed;
    }

    public override void Attack()
    {
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        isAttacking = true;
        agent.velocity = Vector3.zero;
        StopMove();
        transform.LookAt(player);

        yield return new WaitForSeconds( (1/attackRate) / 2);

        var hits = Physics.RaycastAll(attackOriginTransform.position, transform.forward, attackRange, playerLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.ApplyDamage(attackDamage);
                    break;
                }
            }
        }

        yield return new WaitForSeconds( (1/attackRate) / 2);

        StartMove();
        isAttacking = false;
    }
}
