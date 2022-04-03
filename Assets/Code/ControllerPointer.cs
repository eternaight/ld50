using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ControllerPointer : MonoBehaviour
{
    private SpriteAnimator spriteAnimator;
    private Light2D light;

    private float pointerRadius = 1;
    private float pointerLength = 0.5f;
    public float Extent => pointerLength / 2 + pointerRadius;

    private void Start() {
        spriteAnimator = GetComponentInChildren<SpriteAnimator>();
        light = GetComponentInChildren<Light2D>();
    }

    private void Update() {

        var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        var offset = Vector3.ClampMagnitude(cursorPosition - transform.parent.position, pointerRadius);
        transform.localPosition = offset;
        transform.eulerAngles = Vector3.forward * (Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg);
    }

    public void OnHoverOver(IInteractable interactable) {
        if (interactable is Tree) {
            spriteAnimator.PlayClip(1);
        } else {
            spriteAnimator.PlayClip(0);
        }
        light.enabled = true;
    }

    public void SetFlipX(bool flip) => spriteAnimator.SetFlipX(flip);

    public void OnStopHover() {
        spriteAnimator.PlayClip(2);
        light.enabled = false;
    }
}
