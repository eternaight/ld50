using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Bonfire : MonoBehaviour, IInteractable {
    [SerializeField] 
    [Tooltip("Info about bonfire kindling levels.")]
    private BonfireStage[] levels;
    [SerializeField] 
    private float remainingBurnTime;
    [SerializeField]
    private SpriteAnimator spriteAnimator;
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

        OnKindled += UpdateRenderer;
        spriteAnimator.RegisterClips(levels.Select(level => level.clip));
        UpdateRenderer(KindlingLevel, 0);

        while (remainingBurnTime < levels[kindlingLevel].transitionToNextStageSeconds) KindlingLevel--;
    }

    private void OnValidate() {
        ValidateBonfireStages();
    }

    private void Update() {
        remainingBurnTime -= Time.deltaTime;

        if (remainingBurnTime < 0) {
            Burnout();
        } else if (remainingBurnTime < levels[kindlingLevel].transitionToNextStageSeconds) {
            KindlingLevel--;
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

    public void Kindle() {
        remainingBurnTime += levels[kindlingLevel].stickScore;
        SpawnSparks();
        while (kindlingLevel < levels.Length - 1 && remainingBurnTime > levels[kindlingLevel + 1].transitionToNextStageSeconds) KindlingLevel++;
    }

    private void UpdateRenderer(int newKindleLevel, int previousKindling) {
        spriteAnimator.PlayClip(newKindleLevel);
    }

    private void Burnout() {
        enabled = false;
    }

    private void SpawnSparks() {
        var prefab = Resources.Load<GameObject>("Kindle Sparks");
        Instantiate(prefab, transform);
    }

    public void Interact(Controller controller) {
        var topStick = controller.inventory.Pop() as Stick;
        if (topStick != null) {
            topStick.Detach();
            topStick.SetFollow(transform);
        }
    }

    public bool IsInteractable() {
        return enabled;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var stick = collision.GetComponent<Stick>();
        if (stick) {
            Kindle();
            stick.Consume();
        }
    }

    public float GetTemperature(Vector3 point) {
        var baseTemperature = levels[kindlingLevel].temperature;
        var distance = Vector3.Distance(point, transform.position);
        return baseTemperature / (distance + 1);
    }
}
