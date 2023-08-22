using UnityEngine;
using UnityEngine.Events;

namespace EventTriggers
{
    public class CollisionExit3d : MonoBehaviour
    {
        public UnityEvent events;
        void OnCollisionExit(Collision other)
        {
            events?.Invoke();
        }
    }
}