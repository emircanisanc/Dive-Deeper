using UnityEngine;

[System.Serializable]
public class RangedFloat
{
    public float min;
    public float max;

    public float Value{get{return Random.Range(min, max);}}
}
