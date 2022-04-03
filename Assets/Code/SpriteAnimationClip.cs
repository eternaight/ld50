using UnityEngine;

[CreateAssetMenu]
public class SpriteAnimationClip : ScriptableObject {
    public Sprite[] frames;
    [Tooltip("Frames per second")]
    public float framerate;
    private float startTime;

    public float DurationSeconds {
        get {
            if (framerate == 0) return 0;
            return frames.Length / framerate;
        }
    }

    public void SetStartTime() { startTime = Time.time; }

    public Sprite GetFrameAtTime(float time) {
        if (framerate == 0) return frames[0];

        var modDuration = (time - startTime) % DurationSeconds;
        var frameIndex = Mathf.FloorToInt(modDuration * framerate);
        return frames[frameIndex];
    }
}
