using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour {
    [SerializeField]
    private SpriteAnimationClip[] clips;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private int clipNow;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        PlayClip(clipNow);
    }

    private void Update() {
        spriteRenderer.sprite = clips[clipNow].GetFrameAtTime(Time.time);
    }

    private void PlayClip(int clipIndex) {
        if (clipIndex >= 0 && clipIndex < clips.Length) {
            clips[clipIndex].SetStartTime();
            clipNow = clipIndex;
        }
    }

    public void SetFlipX(bool value) => spriteRenderer.flipX = value;
    public bool GetFlipX() => spriteRenderer.flipX;
}

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
