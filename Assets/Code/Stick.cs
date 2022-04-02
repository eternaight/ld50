using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour, IInteractable {
    public void Interact(Controller controller) {
        controller.inventory.AddItem(this);
        gameObject.SetActive(false);
    }
}
