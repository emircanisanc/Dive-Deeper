using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] GameObject explosionParticlePrefab;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed = 2f;
    [SerializeField] float explosionRadius = 3f;
    [SerializeField] float damage = 50f;
    [SerializeField] LayerMask targetLayer;
    bool isDone;

    void Start()
    {
        rb.AddForce(transform.forward * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDone)
            return;

        if (other.CompareTag("EditorOnly"))
            return;

        isDone = true;
        Explode(); 
    }

    private void Explode()
    {
        AudioSource.PlayClipAtPoint(AudioManager.Instance.GrenadeExplosion, transform.position);
        var hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                float netDamage = Mathf.Lerp(0, damage, explosionRadius - Mathf.Clamp(distance, 0, explosionRadius));
                damageable.ApplyDamage(netDamage);
            }
        }
        Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);    
    }
}
