using UnityEngine;

[CreateAssetMenu(menuName = "New MovementStats")]
public class MovementStats : ScriptableObject
{
    public float moveSpeed = 12f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;
    [Range(0, 1)] public float airControl = 0.6f; 
}
