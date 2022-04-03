using UnityEngine;

public interface IInventoryItem {
    void FollowPosition(Vector3 pos);
    Vector3 GetPosition();
    void Consume();
}