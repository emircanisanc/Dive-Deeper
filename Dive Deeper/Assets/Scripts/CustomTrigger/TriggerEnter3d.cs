using UnityEngine;
using UnityEngine.Events;

namespace EventTriggers
{
    public class TriggerEnter3d : MonoBehaviour
    {
        public UnityEvent events;
        void OnTriggerEnter(Collider other)
        {
            events?.Invoke();
        }
    }
}