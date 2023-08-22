using UnityEngine;

public class LifeTimeDestroy : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] private float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);    
    }

}