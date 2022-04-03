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
    [SerializeField]
    private bool playOnStart;

    private void Awake() {
        enabled = playOnStart;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        if (playOnStart) {
            PlayClip(clipNow);
        }
    }

    private void Update() {
        spriteRenderer.sprite = clips[clipNow].GetFrameAtTime(Time.time);
    }

    public void Stop() {
        enabled = false;
    }

    public void PlayClip(int clipIndex) {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;
        }
        if (clipNow == clipIndex) return;
        
        enabled = true;

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
