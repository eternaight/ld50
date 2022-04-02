using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Bonfire : MonoBehaviour, IInteractable {
    [SerializeField] 
    [Tooltip("Info about bonfire kindling levels.")]
    private BonfireStage[] levels;
    [SerializeField] 
    private float remainingBurnTime;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Light2D spriteLight;

    public delegate void BonfireKindleAction(int newKindleLevel, int previousKindling);
    public event BonfireKindleAction OnKindled;

    private int kindlingLevel;
    public int maxKindling;
    public int KindlingLevel {
        get {
            return kindlingLevel;
        }
        private set {
            if (kindlingLevel == value) return;
            OnKindled?.Invoke(value, kindlingLevel);
            kindlingLevel = value;
        }
    }

    private void Awake() {
        KindlingLevel = levels.Length - 1;
        maxKindling = levels.Length - 1;
    }

    private void Start() {
        ValidateBonfireStages();

        while (remainingBurnTime < levels[kindlingLevel].transitionToNextStageSeconds) KindlingLevel--;
        UpdateRenderer();
    }

    private void OnValidate() {
        ValidateBonfireStages();
        if (spriteRenderer != null && levels != null && levels.Length > 0 && levels[levels.Length - 1] != null) spriteRenderer.sprite = levels[levels.Length - 1]?.sprite;
    }

    private void Update() {
        remainingBurnTime -= Time.deltaTime;

        if (remainingBurnTime < 0) {
            Burnout();
        } else if (remainingBurnTime < levels[kindlingLevel].transitionToNextStageSeconds) {
            KindlingLevel--;
            UpdateRenderer();
        }
    }

    private void ValidateBonfireStages() {
        if (levels is null) return;
        if (levels.Length < 2) return;
        foreach (var item in levels) {
            if (item is null) return;
        }
        Array.Sort(levels);
        levels[0].transitionToNextStageSeconds = 0;
    }

    public void Kindle(float seconds) {
        remainingBurnTime += seconds;
        while (kindlingLevel < levels.Length - 1 && remainingBurnTime > levels[kindlingLevel + 1].transitionToNextStageSeconds) KindlingLevel++;
        UpdateRenderer();
    }

    private void UpdateRenderer() {
        spriteRenderer.sprite = levels[kindlingLevel].sprite;
    }

    private void Burnout() {
        enabled = false;
    }

    public void Interact(Controller controller) {
        if (controller.inventory.TryGetStick()) {
            Kindle(10);
        }
    }
}
