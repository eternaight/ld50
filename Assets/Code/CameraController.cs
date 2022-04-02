using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] 
    private Vector2 worldMin;
    [SerializeField]
    private Vector2 worldMax;

    private Vector2 camHalfSize;

    private void Start() {
        camHalfSize = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane)) - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        camHalfSize *= 0.51f;
    }

    private void Update() {
        var position = transform.parent.position + Vector3.back * 10;
        position.x = Mathf.Clamp(position.x, worldMin.x + camHalfSize.x, worldMax.x - camHalfSize.x);
        position.y = Mathf.Clamp(position.y, worldMin.y + camHalfSize.y, worldMax.y - camHalfSize.y);
        transform.localPosition = transform.parent.InverseTransformPoint(position);
    }
}
