using System.Collections;
using UnityEngine;

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

    private void Start() {
        bonfire = GetComponentInParent<Bonfire>();

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
        var w = Mathf.PerlinNoise(Time.time * beaconFrequency, 0) * beaconScale * kindlingModifier * animModifier;
        transform.localScale = new Vector3(w, w, 1);
    }
}