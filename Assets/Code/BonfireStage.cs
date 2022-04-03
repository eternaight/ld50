using System;
using UnityEngine;

[CreateAssetMenu]
public class BonfireStage : ScriptableObject, IComparable<BonfireStage>
{
    [Header("Bonfire sprite at this stage")] 
    public SpriteAnimationClip clip;
    [Header("Seconds remaining when this stage stops")] 
    public float transitionToNextStageSeconds;
    [Header("Seconds added to the bonfire by feeding it sticks")]
    public float stickScore;
    [Header("Temperature of the bonfire at distance 0")]
    public float temperature;

    public int CompareTo(BonfireStage obj) {
        if (obj == null) return 1;
        if (transitionToNextStageSeconds == obj.transitionToNextStageSeconds) return 0;
        return (transitionToNextStageSeconds > obj.transitionToNextStageSeconds ? 1 : -1);
    }
}
