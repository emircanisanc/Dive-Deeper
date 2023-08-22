using UnityEngine;
using UnityEngine.Events;

namespace EventTriggers
{
    public class TriggerExit3d : MonoBehaviour
    {
        public UnityEvent events;
        void OnTriggerExit(Collider other)
        {
            events?.Invoke();
        }
    }
}