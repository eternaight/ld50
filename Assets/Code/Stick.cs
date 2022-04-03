using UnityEngine;

public class Stick : MonoBehaviour, IInteractable, IInventoryItem {
    private const float stickSpeed = 10;
    private const float minSeparation = .5f;
    private Transform followTarget;
    private bool interactable = true;

    public void Consume() {
        Detach();
        Destroy(gameObject);
    }
    
    public void Detach() {
        Inventory.Main.Remove(this);
    }

    public void Update() {
        if (followTarget != null) {
            FollowPosition(followTarget.position);
        }
    }

    public void SetFollow(Transform tf) => followTarget = tf;

    private void FollowPosition(Vector3 pos) {
        var distance = Vector3.Distance(pos, transform.position);
        if (Mathf.Abs(distance - minSeparation) < 0.01f) {
            return;
        }
        distance -= minSeparation;
        transform.position = Vector3.MoveTowards(transform.position, pos, stickSpeed * Time.deltaTime * distance);
    }

    public Transform GetTransform() => transform;

    public void Interact(Controller controller) {
        controller.inventory.AddItem(this);
        interactable = false;
    }

    public bool IsInteractable() {
        return interactable;
    }
}
