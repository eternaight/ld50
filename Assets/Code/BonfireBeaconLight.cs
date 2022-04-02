using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BonfireBeaconLight : MonoBehaviour
{
    [SerializeField]
    private float beaconScale;
    [SerializeField]
    private float beaconFrequency;
    public float kindlingModifier;
    public float animModifier;

    [SerializeField]
    private AnimationCurve lightUpCurve;
    [SerializeField]
    private float lightUpTime;
    [SerializeField]
    private float lightUpMultilplier;
    [SerializeField]
    private AnimationCurve dipDownCurve;
    [SerializeField]
    private float dipDownTime;

    private Bonfire bonfire;
    private Light2D[] bonfireLights;

    private void Start() {
        bonfire = GetComponentInParent<Bonfire>();
        bonfireLights = bonfire.GetComponentsInChildren<Light2D>();

        if (bonfire == null) { enabled = false; return; }

        bonfire.OnKindled += OnKindled;

        kindlingModifier = bonfire.KindlingLevel / bonfire.maxKindling;
        animModifier = 1;
    }

    private void Update() {
        AnimateBeaconLight();
    }

    private void OnKindled(int newKindlingLevel, int oldKindling) {
        kindlingModifier = (float)newKindlingLevel / bonfire.maxKindling;
        if (newKindlingLevel > oldKindling) {
            StopAllCoroutines();
            StartCoroutine(LightUpCoroutine());
        } else {
            StopAllCoroutines();
            StartCoroutine(DipDownCoroutine());
        }
    }

    private IEnumerator LightUpCoroutine() {
        var delay = new WaitForSeconds(lightUpTime / 256);
        for (int i = 1; i < 257; i++) {
            animModifier = 1 + lightUpCurve.Evaluate(i / 256f) * lightUpMultilplier;
            yield return delay;
        }
        animModifier = 1;
    }
    private IEnumerator DipDownCoroutine() {
        var delay = new WaitForSeconds(lightUpTime / 256);
        for (int i = 1; i < 257; i++) {
            kindlingModifier = 1 - lightUpCurve.Evaluate(i / 256f);
            yield return delay;
        }
        animModifier = 1;
    }

    private void AnimateBeaconLight() {
        // beacon position
        var offset = transform.parent.position;
        var viewportMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        var viewportMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        viewportMin.z = 0;
        viewportMax.z = 0;
        offset.x = Mathf.Clamp(offset.x, viewportMin.x, viewportMax.x);
        offset.y = Mathf.Clamp(offset.y, viewportMin.y, viewportMax.y);
        transform.position = offset;

        // light radius
        var w = Mathf.PerlinNoise(Time.time * (beaconFrequency * Mathf.PerlinNoise(0.23f, Time.time)), 0.34f) * beaconScale * kindlingModifier * animModifier;
        
        foreach (Light2D light in bonfireLights) {
            light.pointLightOuterRadius = w;
            light.intensity = 0.75f * animModifier;
        }
    }
}
