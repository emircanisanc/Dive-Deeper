using UnityEngine;

[System.Serializable]
public class RangedInt
{
    public int min;
    public int max;

    public int Value{get{return Random.Range(min, max);}}
}
