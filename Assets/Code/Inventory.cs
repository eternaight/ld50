using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private static Inventory main;
    public static Inventory Main {
        get {
            if (main == null) new Inventory();
            return main;
        }
    }

    private readonly List<IInventoryItem> convoy;

    public int GetInventoryCount() { return convoy.Count; }

    public event System.Action OnInventoryChange;

    private Inventory() {
        main = this;
        convoy = new List<IInventoryItem>();
    }

    public void UpdateConvoyPositions(Vector3 headPosition) {
        foreach (var item in convoy) {
            item.FollowPosition(headPosition);
            headPosition = item.GetPosition();
        }
    }

    public void AddItem(IInventoryItem item) {
        if (convoy.Contains(item)) return;

        if (item is Stick) {
            convoy.Add(item);
        }
    }

    public bool TryGetStick() {
        if (convoy.Count > 0) {
            var item = convoy[convoy.Count - 1];
            item.Consume();
            convoy.RemoveAt(convoy.Count - 1);
            OnInventoryChange?.Invoke();
            return true;
        }
        return false;
    }
}
