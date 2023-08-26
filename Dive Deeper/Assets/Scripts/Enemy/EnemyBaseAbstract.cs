using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

public abstract class EnemyBaseAbstract : MonoBehaviour, IDamageable, IHitable
{
    public static Action AnEnemyDied;

    [SerializeField] protected float maxHealth;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float attackRate = 2f;
    [SerializeField] protected int attackDamage = 10;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected Transform attackOriginTransform;
    [SerializeField] protected int attackAnimCount;

    protected Collider coll;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected static Transform player;
    protected Vector3 playerPos;
    protected float nextAttackTime;
    protected bool isAttacking;
    protected bool canMove;
    protected bool isDead;
    protected float moveSpeed;
    protected float currentHealth;
    public bool autoActivate;

    protected virtual void Awake()
    {
        coll = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        moveSpeed = agent.speed;
    }
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        coll.enabled = true;
        animator.SetFloat("MoveSpeed", moveSpeed);
        agent.speed = moveSpeed;
        if (player == null)
        {
            player = PlayerHealth.Instance.transform;
        }
        if (autoActivate)
            SetPlayer();
    }

    void Update()
    {
        UpdateMethod();    
    }

    public void SetPlayer()
    {
        enabled = true;
        StartChasing();
    }

    protected void StopChasing()
    {
        StopMove();
        canMove = false;   
    }
    private void StartChasing()
    {
        if(isDead)
            return;
        StartMove();
        canMove = true;
    }
    protected void StopMove()
    {
        animator.SetBool("IsWalking", false);
        agent.isStopped = true;
    }
    protected void StartMove()
    {
        animator.SetBool("IsWalking", true);
        agent.isStopped = false;
    }

    public virtual void UpdateMethod()
    {
        if(!canMove)
            return;
        if (!isAttacking)
        {
            playerPos = player.position;
            float distanceToPlayer = Vector3.Distance(transform.position, playerPos);
            agent.SetDestination(playerPos);
            if (distanceToPlayer <= attackRange)
            {
                FaceTarget();
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                    var attackAnim = "Attack" + UnityEngine.Random.Range(0, attackAnimCount);
                    animator.SetTrigger(attackAnim);
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
    }

    public abstract void Attack();
    protected virtual void Die()
    {
        coll.enabled = false;
        isDead = true;
        isAttacking = false;
        AnEnemyDied?.Invoke();
        StopAllCoroutines();
        StopChasing();
        Disappear();
        //animator.SetTrigger("Die");
        //Invoke("Disappear", 2f);
    }
    private void Disappear()
    {
        Destroy(gameObject);
    }
    protected void FaceTarget()
    {
        Vector3 dir = (playerPos - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;

        if (!canMove)
            StartChasing();

        if (currentHealth <= 0)
        {
            EnemySpawner.Instance.OnEnemyDied();
            Die();
        }
    }

    public void Hit(RaycastHit hit)
    {
        GunEffect.CreateEffect(hit.point, hit.normal);
    }
}