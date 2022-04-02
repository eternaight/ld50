using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Info about bonfire stages.")]
    private BonfireStage[] stages;
    [SerializeField] 
    private float remainingBurnTime;
    [SerializeField] 
    private Transform bonfireLightTransform;
    [SerializeField]
    private float bonfireLightFrequency;
    [SerializeField]
    private float bonfireLightScale;

    private int currentStage;
    private SpriteRenderer spriteRenderer;
    
    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentStage = stages.Length - 1;

        ValidateBonfireStages();

        while (remainingBurnTime < stages[currentStage].startBurnTime) currentStage--;
    }

    private void OnValidate() {
        ValidateBonfireStages();

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = stages[stages.Length - 1].sprite;
    }

    private void Update() {
        remainingBurnTime -= Time.deltaTime;

        if (remainingBurnTime <= 0) {
            Burnout();
        } else if (remainingBurnTime < stages[currentStage].startBurnTime) currentStage--;

        AnimateLight();
    }

    private void ValidateBonfireStages() {
        if (stages is null) return;
        if (stages.Length < 2) return;
        foreach (var item in stages) {
            if (item is null) return;
        }
        Array.Sort(stages);
        stages[0].startBurnTime = 0;
    }

    private void TrySetStage(int newStage) {
        if (newStage == currentStage) return;
        currentStage = newStage;
        spriteRenderer.sprite = stages[currentStage].sprite;
    }

    public void Feed(float seconds) {
        remainingBurnTime += seconds;
        while (currentStage < stages.Length - 1 && remainingBurnTime > stages[currentStage + 1].startBurnTime) currentStage++;
    }

    private void AnimateLight() {
        var w = Mathf.PerlinNoise(Time.time * bonfireLightFrequency, 0) * bonfireLightScale;
        bonfireLightTransform.localScale = new Vector3(w, w, 1);
    }

    private void Burnout() {
        enabled = false;
        bonfireLightTransform.localScale = Vector3.zero;
    }
}
