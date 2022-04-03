using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPointer : MonoBehaviour
{
    private SpriteAnimator spriteAnimator;

    private float pointerRadius = 1;
    private float pointerLength = 0.5f;
    public float Extent => pointerLength / 2 + pointerRadius;

    private void Start() {
        spriteAnimator = GetComponentInChildren<SpriteAnimator>();
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
    }

    public void SetFlipX(bool flip) => spriteAnimator.SetFlipX(flip);

    public void OnStopHover() => spriteAnimator.PlayClip(2);
}
