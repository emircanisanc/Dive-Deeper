using UnityEngine;
using UnityEngine.Events;

namespace EventTriggers
{
public class ColliderEnter3d : MonoBehaviour
{
    public UnityEvent events;

    void OnCollisionEnter(Collision other)
    {
        events?.Invoke();
    }
}
}
