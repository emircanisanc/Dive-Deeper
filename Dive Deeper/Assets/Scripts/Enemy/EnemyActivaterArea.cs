using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivaterArea : MonoBehaviour
{
    [SerializeField] bool autoGetEnemies;
    [SerializeField] float autoGetRadius = 1f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] List<EnemyBaseAbstract> enemies;

    bool isDone;


    void Awake()
    {
        if (autoGetEnemies)
        {
            enemies = new List<EnemyBaseAbstract>();
            foreach (var coll in Physics.OverlapSphere(transform.position, autoGetRadius, enemyLayer))
            {
                enemies.Add(coll.GetComponent<EnemyBaseAbstract>());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDone)
            return;

        if (other.CompareTag("Player"))
        {
            isDone = true;
            foreach (EnemyBaseAbstract enemy in enemies)
            {
                if (enemy)
                    enemy.SetPlayer();
            }
            Destroy(gameObject);
        }    
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, autoGetRadius);    
    }
}
