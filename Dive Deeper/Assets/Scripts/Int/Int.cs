using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Int")]
public class Int : ScriptableObject
{
    [SerializeField] private int value;
    public int Value{get{return value;} set{this.value = value; OnValueChanged?.Invoke(value);}}
    public UnityEvent<int> OnValueChanged;
}
