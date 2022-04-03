using UnityEngine;

public interface IInventoryItem {
    void SetFollow(Transform pos);
    Transform GetTransform();
    void Consume();
}