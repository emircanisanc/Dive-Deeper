using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Float")]
public class Float : ScriptableObject
{
    [SerializeField] private float value;
    public float Value{get{return value;} set{this.value = value; OnValueChanged?.Invoke(value);}}
    public UnityEvent<float> OnValueChanged;
}
