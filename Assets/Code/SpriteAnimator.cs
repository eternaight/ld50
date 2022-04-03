using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour {
    [SerializeField]
    private List<SpriteAnimationClip> clips;
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

    public void PlayClip(int clipIndex) {
        if (clipIndex >= 0 && clipIndex < clips.Count) {
            clips[clipIndex].SetStartTime();
            clipNow = clipIndex;
        }
    }

    public void RegisterClips(IEnumerable<SpriteAnimationClip> clips) {
        this.clips.AddRange(clips);
    }

    public void SetFlipX(bool value) => spriteRenderer.flipX = value;
    public bool GetFlipX() => spriteRenderer.flipX;
}
