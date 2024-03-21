using UnityEngine;

public class InventorySlot : Slot {
    private Item containedItem;
    public ItemType type;

    public bool HaveItem() {
        return !(containedItem == null);
    }

    public void SetItem(Item itemToSet) {
        if (IsDisabled()) {
            return;
        }
        containedItem = itemToSet;
        if (itemToSet != null && itemToSet.large) {
            InventorySlotManager.GetSlotFromIndex(index++).Disable();
        }
        //SetImage(itemToSet?.icon);
    }

    public Item GetItem() {
        return containedItem;
    }

    public override void Disable() {
        _Disable();
        SetItem(null);
    }
}