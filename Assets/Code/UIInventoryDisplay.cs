using UnityEngine;
using UnityEngine.UI;

public class UIInventoryDisplay : MonoBehaviour
{
    private Text text;

    private void Start() {
        text = GetComponentInChildren<Text>();
        Inventory.Main.OnInventoryChange += UpdateInventoryDisplay;
        UpdateInventoryDisplay();
    }

    private void UpdateInventoryDisplay() {
        text.text = $"Sticks: {Inventory.Main.GetInventoryCount()}";
    }
}
