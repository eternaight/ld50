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

    public void UpdateConvoyPositions(Transform headTf) {
        foreach (var item in convoy) {
            item.SetFollow(headTf);
            headTf = item.GetTransform();
        }
        
        for (int i = 0; i < convoy.Count;) {
            if (convoy[i] != null) {
                i++; 
            } else {
                convoy.RemoveAt(i);
            }
        }
    }

    public void AddItem(IInventoryItem item) {
        if (convoy.Contains(item)) return;

        if (item is Stick) {
            convoy.Add(item);
        }
    }

    public IInventoryItem Pop() {
        if (convoy.Count > 0) {
            var item = convoy[convoy.Count - 1];
            OnInventoryChange?.Invoke();
            return item;
        }
        return null;
    }

    public bool Remove(IInventoryItem item) {
        return convoy.Remove(item);
    }
}
