using UnityEngine;

public class WeaponSlot : Slot {
    private Weapon containedWeapon;

    public Weapon GetWeapon() {
        return containedWeapon;
    }

    public void SetWeapon(Weapon weaponToSet) {
        if (IsDisabled()) {
            return;
        }
        containedWeapon = weaponToSet;
    }

    public override void Disable() {
        _Disable();
        SetWeapon(null);
    }
}