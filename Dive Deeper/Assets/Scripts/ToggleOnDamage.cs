using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToggleOnDamage : MonoBehaviour, IDamageable
{
    public GameObject[] gameObjectsToToggle;
    public Collider coll;
    public NavMeshObstacle navMeshObstacle;
    bool isDone;

    public void ApplyDamage(float damage)
    {
        if (isDone)
            return;

        isDone = true;
        navMeshObstacle.enabled = false;
        coll.enabled = false;
        foreach (var gameObject in gameObjectsToToggle)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
