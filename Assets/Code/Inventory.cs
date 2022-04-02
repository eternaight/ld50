using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private int stickCount;

    public void AddItem(IInteractable item) {
        if (item is Stick) {
            stickCount++;
        }
    }

    public bool TryGetStick() {
        if (stickCount > 0) {
            stickCount--;
            return true;
        }
        return false;
    }
}
