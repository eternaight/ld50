using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    [SerializeField] private float windDirectionFrequency;

    private ParticleSystem particles;

    private void Start() {
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update() {
    }
}
