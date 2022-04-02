using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireSpriteLightController : MonoBehaviour
{
    private void Start() {
        var bonfire = GetComponentInParent<Bonfire>();
        bonfire.OnKindled += Bonfire_OnKindled;
    }

    private void Bonfire_OnKindled(int newKindleLevel, int previousKindling) {
        SetActiveSprite(newKindleLevel);
    }

    private void SetActiveSprite(int newKindleLevel) {
        foreach (Transform tf in transform) {
            tf.gameObject.SetActive(false);
        }
        transform.GetChild(newKindleLevel).gameObject.SetActive(true);
    }
}
