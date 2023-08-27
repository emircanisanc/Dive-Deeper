using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class Necromancer : MeleeEnemy
{
    AudioSource audioSource;

    public string bossName = "NECROMANCER";
    public GameObject spawnOnDeath;
    public float spawnDeltaY = 1f;

    [Header("Projectile")]
    public AudioClip throwClip;
    public SphereCollider projectileTrigger;
    public Transform projectileTargetPos;
    public float projectileRange = 5f;
    public float projectileThrowDuration = 8f;
    public float throwAnimDuration = 5f;
    float nextThrowTime;
    Vector3 startLocalPos;
    Sequence throwSequence;

    [Header("Spawning Enemies")]
    public AudioClip spellClip;
    public float spawnSkeletonDuration = 5f;
    public float spawnSkeletonAnimDuration = 5f;
    public GameObject[] skeletonPrefabs;
    public int minSpawnCount = 1;
    public int maxSpawnCount = 3;
    float nextSpawnTime;
    List<EnemyBaseAbstract> spawnedEnemies;
    Coroutine spawnCoroutine;

    protected override void Awake()
    {
        base.Awake();
        spawnedEnemies = new List<EnemyBaseAbstract>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        AudioManager.Instance.OnSoundVolumeChanged += ChangeSoundVolume;
        audioSource.volume = AudioManager.Instance.SoundVolume;
    }


    void OnDestroy()
    {
        if (AudioManager.Instance)
            AudioManager.Instance.OnSoundVolumeChanged -= ChangeSoundVolume;
    }

    private void ChangeSoundVolume(float value)
    {
        audioSource.volume = value;
    }
    private void UpdateAudioSource(float value)
    {

    }

    protected override void Start()
    {
        base.Start();
        nextSpawnTime = Time.time + spawnSkeletonDuration;
        nextThrowTime = Time.time + projectileThrowDuration;
        startLocalPos = projectileTrigger.transform.localPosition;

        
    }

    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);
        InGameUI.Instance.SetPercentOfHealthBar(currentHealth / maxHealth);
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
            if (Time.time >= nextSpawnTime)
            {
                isAttacking = true;
                agent.velocity = Vector3.zero;
                StopMove();
                transform.LookAt(player);
                spawnCoroutine = StartCoroutine(SpawnSkeletonCoroutine());
            }
            else if (Time.time >= nextThrowTime && distanceToPlayer <= projectileRange)
            {
                isAttacking = true;
                agent.velocity = Vector3.zero;
                StopMove();
                transform.LookAt(player);
                ThrowWeapon();
            }
            else if (distanceToPlayer <= attackRange)
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

    protected void ThrowWeapon()
    {
        animator.SetTrigger("Attack1");
        projectileTrigger.enabled = true;
        throwSequence = DOTween.Sequence();
        throwSequence.Append(transform.DOScale(transform.localScale, 0.58f).OnComplete(() => PlayClip(throwClip)));
        throwSequence.Append(projectileTrigger.transform.DOLocalMove(projectileTargetPos.localPosition, 0.46f).SetEase(Ease.Linear));
        throwSequence.Append(transform.DOScale(transform.localScale, 2.47f));
        throwSequence.Append(projectileTrigger.transform.DOLocalMove(startLocalPos, 0.5f).SetEase(Ease.Linear));
        throwSequence.OnComplete(() => StopThrowing());
    }

    public override void SetPlayer()
    {
        base.SetPlayer();
        InGameUI.Instance.OpenBossHealthBar(bossName);
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void StopThrowing()
    {
        nextThrowTime = Time.time + projectileThrowDuration;
        projectileTrigger.enabled = false;
        isAttacking = false;
        StartMove();
    }

    IEnumerator SpawnSkeletonCoroutine()
    {
        animator.SetTrigger("Spell");
        PlayClip(spellClip);
        yield return new WaitForSeconds(spawnSkeletonAnimDuration - 1);
        var spawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * Random.Range(0.5f, 1.5f);
            GameObject enemyPrefab = skeletonPrefabs[Random.Range(0, skeletonPrefabs.Length)];
            spawnedEnemies.Add(Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<EnemyBaseAbstract>());
        }
        yield return new WaitForSeconds(1);
        StartMove();
        nextSpawnTime = Time.time + spawnSkeletonDuration;
        isAttacking = false;
        spawnCoroutine = null;
    }

    protected override void Die()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        if (throwSequence != null)
            throwSequence.Kill();


        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (spawnedEnemies[i] != null)
            {
                spawnedEnemies[i].ApplyDamage(1000);
            }
        }
        StopThrowing();
        Vector3 spawnPos = transform.position;
        spawnPos.y += spawnDeltaY;
        if (spawnOnDeath)
            Instantiate(spawnOnDeath, spawnPos, Quaternion.identity);
        coll.enabled = false;
        isDead = true;
        isAttacking = false;
        AnEnemyDied?.Invoke();
        StopAllCoroutines();
        StopChasing();
        animator.SetTrigger("Die");
        Invoke("Disappear", 2f);
    }

}
