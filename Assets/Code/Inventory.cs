public class Inventory
{
    private static Inventory main;
    public static Inventory Main {
        get {
            if (main == null) new Inventory();
            return main;
        }
    }

    private int stickCount;
    public int GetInventoryCount() { return stickCount; }

    public event System.Action OnInventoryChange;

    private Inventory() {
        main = this;
    }

    public void AddItem(IInteractable item) {
        if (item is Stick) {
            stickCount++;
            OnInventoryChange();
        }
    }

    public bool TryGetStick() {
        if (stickCount > 0) {
            stickCount--;
            OnInventoryChange();
            return true;
        }
        return false;
    }
}
