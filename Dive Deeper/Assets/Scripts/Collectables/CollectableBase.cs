using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBase : MonoBehaviour
{
    [SerializeField] string messageOnCollected = "Collected: bla bla";
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayPickupAtPoint(transform.position);
            Message.ShowMessageInstance(messageOnCollected);
            Collect(other);
        }    
    }

    protected virtual void Collect(Collider other)
    {
        AfterCollect();
    }

    protected void AfterCollect()
    {
        // play sound
        Destroy(gameObject);
    }
}
