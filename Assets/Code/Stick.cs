using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour, IInteractable, IInventoryItem {
    private const float stickSpeed = 10;
    private const float minSeparation = .5f;

    public void Consume() {
        gameObject.SetActive(false);
    }

    public void FollowPosition(Vector3 pos) {
        var distance = Vector3.Distance(pos, transform.position);
        if (distance < minSeparation) return;
        distance -= minSeparation;
        transform.position = Vector3.MoveTowards(transform.position, pos, stickSpeed * Time.deltaTime * distance);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Interact(Controller controller) {
        controller.inventory.AddItem(this);
    }
}
