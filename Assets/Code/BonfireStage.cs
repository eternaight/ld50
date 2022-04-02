using System;
using UnityEngine;

[CreateAssetMenu]
public class BonfireStage : ScriptableObject, IComparable<BonfireStage>
{
    [Header("Bonfire sprite at this stage")] 
    public Sprite sprite;
    [Header("Seconds remaining when this stage stops")] 
    public float startBurnTime;

    public int CompareTo(BonfireStage obj) {
        if (obj == null) return 1;
        if (startBurnTime == obj.startBurnTime) return 0;
        return (startBurnTime > obj.startBurnTime ? 1 : -1);
    }
}
