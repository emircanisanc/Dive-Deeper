using UnityEngine;

public class Wall : MonoBehaviour, IHitable
{
    [SerializeField] private GameObject holePrefab;

    public void Hit(RaycastHit hit)
    {
        Instantiate(holePrefab, hit.point + (hit.normal * 0.001f), Quaternion.FromToRotation(Vector3.up, hit.normal));
    }

}