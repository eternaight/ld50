using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour, IInteractable {
    [SerializeField] 
    [Tooltip("Info about bonfire kindling levels.")]
    private BonfireStage[] levels;
    [SerializeField] 
    private float remainingBurnTime;
    [SerializeField] 
    private Transform bonfireLightTransform;
    [SerializeField]
    private float bonfireLightFrequency;
    [SerializeField]
    private float bonfireLightScale;

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

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        KindlingLevel = levels.Length - 1;
        maxKindling = levels.Length - 1;
    }

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        ValidateBonfireStages();

        while (remainingBurnTime < levels[kindlingLevel].transitionToNextStageSeconds) KindlingLevel--;
        UpdateRenderer();
    }

    private void OnValidate() {
        var sr = GetComponent<SpriteRenderer>();
        ValidateBonfireStages();
        if (sr != null && levels != null && levels.Length > 0 && levels[levels.Length - 1] != null) sr.sprite = levels[levels.Length - 1]?.sprite;
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
        bonfireLightTransform.localScale = Vector3.zero;
        CancelInvoke("AnimateLight");
    }

    public void Interact(Controller controller) {
        if (controller.inventory.TryGetStick()) {
            Kindle(10);
        }
    }
}
