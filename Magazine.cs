using UnityEngine;

public class Magazine : Item {
    private int capacity;
    public int maxCapacity;
    public MagazineType magType;

    void Awake() {
        capacity = maxCapacity;
    }

    private void OnChangeCapacity() {
        slot.UpdateSlider((float)capacity);
    }

    public void Unload(int toUnload) {
        capacity -= toUnload;
    }
    
    public void Load(int toLoad) {
        
    }

    public int GetCapacity() {
        return capacity;
    }
}