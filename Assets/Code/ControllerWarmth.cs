using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWarmth : MonoBehaviour {

    public float warmth;
    private bool warming;

    [SerializeField] private SpriteAnimator spriteAnimator;
    [SerializeField] private Bonfire bonfire;
    [SerializeField] private ParticleSystem freezingParticles;
    
    private void Update() {
        var ambientTemperature = bonfire.GetTemperature(transform.position) - 20;
        warmth = Mathf.MoveTowards(warmth, ambientTemperature, Time.deltaTime * 10);
        
        if (ambientTemperature > 0 != warming) {
            spriteAnimator.PlayClip(ambientTemperature > 0 ? 1 : 0);
            warming = ambientTemperature > 0;
        }

        var em = freezingParticles.emission;
        em.rateOverTime = Mathf.Max(-warmth * 0.1f, 0);
    }
}
