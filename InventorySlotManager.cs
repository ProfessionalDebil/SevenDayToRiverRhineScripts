using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class InventorySlotManager : MonoBehaviour {
    // primary = 7 slot
    // underbarrel = 12 slot
    // secondary = 6
    // launcher = 3
    // others = 38
    // total = 7 + 12 + 6 + 3 + 38 = 66

    [SerializeField]
    private Canvas inventoryCanvas;
    private bool inventoryOpened = false;
    public List<InventorySlot> slotContainer;
    public List<InventorySlot> weaponSlots;
    private static InventorySlotManager objReference;
    [SerializeField]

    void Update() {
        if (Input.GetButtonDown("InventoryOpen")) {
            inventoryOpened = !inventoryOpened;
            inventoryCanvas.enabled = inventoryOpened;
        }
    }

    public void Awake() {
        objReference = this;
        for (int i = 0; i < slotContainer.Count; i++) {
            slotContainer[i].index = i;
        }
        /*for (int i = 0; i <= weaponSlots.Count; i++) {
            weaponSlots[i].index = i;
        }*/
    }

    public static void BakeLoadout(Loadout loadout) {
        List<Item> loadoutSets = new List<Item>(loadout.primary);
        loadoutSets.AddRange(loadout.underbarrel);
        loadoutSets.AddRange(loadout.secondary);
        loadoutSets.AddRange(loadout.launcher);
        loadoutSets.AddRange(loadout.others);

        for (int i = 0; i < loadoutSets.Count; i++) {
            Debug.Log(loadoutSets[i]);
            Put(loadoutSets[i], objReference.slotContainer[i]);
        }
    }
    
    public static Item Take(Item toTake) {
        if (toTake == null) {
            return null;
        }
        InventorySlot slot = toTake.slot;
        slot.SetItem(null);
        slot.SetActiveSlider(false); // turn off the sprite and image component
        toTake.slot = null;
        //toTake.GetComponent<Renderer>().enabled = true;
        return toTake; // make new return thingy, if called wrongly, the item may be gone forever
    }
    

    public static bool Put(Item toPut) {
        if (toPut == null) {
            return false;
        }
        InventorySlot slot = GetEmptySlotForType(toPut.type);
        if (slot == null) {
            return false;
        }
        slot.SetItem(toPut);
        slot.SetActiveSlider(true);
        slot.SetIcon(toPut.icon);
        //toPut.GetComponent<Renderer>().enabled = false;
        toPut.slot = slot;
        return true;
    }

    public static bool Put(Item toPut, InventorySlot slot) {
        Debug.Log(toPut);
        Debug.Log(toPut.slot);
        Debug.Log(slot);
        if (toPut == null || slot == null) {
            Debug.Log("return");
            return false;
        }
        slot.SetItem(toPut);
        slot.SetActiveSlider(true);
        slot.SetIcon(toPut.icon);
        //toPut.GetComponent<Renderer>().enabled = false;
        toPut.slot = slot;
        return true;
    }

    static public InventorySlot GetSlotFromIndex(int index) {
        if (index < 0 || index >= 66) {
            Debug.LogException(new ArgumentException("Slot index out of bounds"));
        }
        return objReference.slotContainer[index];
    }

    static public InventorySlot GetEmptySlotForType(ItemType type) {
        int min = 28;
        int max = 65;
        switch (type) {
            case ItemType.Primary:
                min = 0;
                max = 6;
                break;
            case ItemType.Underbarrel:
                min = 7;
                max = 18;
                break;
            case ItemType.Secondary:
                min = 19;
                max = 24;
                break;
            case ItemType.Launcher:
                min = 25;
                max = 27;
                break;
            case ItemType.Others:
                min = 28;
                max = 65;
                break;
        }

        for (int i = min; i <= max; i++) {
            if (objReference.slotContainer[i].GetItem() == null) {
                return objReference.slotContainer[i];
            }
        }
        return null;
    }

    static public Magazine GetMagazineToReload(MagazineType magType) {
        IEnumerable<Magazine> strippedInventory = objReference.GetMagazineFromInventory().Where(mag => mag.magType == magType).OrderByDescending(mag => mag.GetCapacity());
        return strippedInventory.ElementAt(0);
        /*Magazine magToUse = null;
        foreach (Magazine mag in objReference.GetMagazineFromInventory()) {
            if (mag.magType == magType && mag.GetCapacity() > (magToUse == null ? magToUse.GetCapacity() : 0)) {
                magToUse = mag;
            }
        }
        return magToUse;*/
    }

    private IEnumerable<InventorySlot> GetStrippedInventory() {
        IEnumerable<InventorySlot> strippedInventory = slotContainer.Where(slot => slot.GetItem() != null);
        return strippedInventory;
        /*foreach (InventorySlot invSlot in slotContainer) {
            if (invSlot.GetItem() == null) {
                continue;
            }
            yield return invSlot;
        }*/
    }

    private IEnumerable<Magazine> GetMagazineFromInventory() {
        IEnumerable<Item> items = GetStrippedInventory().Select(slot => slot.GetItem()).Where(item => item is Magazine);
        IEnumerable<Magazine> mags = items.Select(item => item as Magazine);
        return mags;
        /*foreach (InventorySlot invSlot in GetStrippedInventory()) {
            Item item = invSlot.GetItem();
            if (item is Magazine) {
                continue;
            }
            yield return item as Magazine;
        }*/
    }
}