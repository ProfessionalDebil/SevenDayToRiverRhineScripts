using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Weapon weapon;
    private float nextFire;
    private bool isAiming = false;
    public Loadout startingLoadout;

    void Start() {
        InventorySlotManager.BakeLoadout(startingLoadout);
    }

    void Update() {
        if (weapon != null) {
            if (Input.GetButton("Fire1") && weapon.CanFire(nextFire)) {
                weapon.Fire();
                nextFire = Time.time + weapon.values.fireRate;
            }
            if (Input.GetButtonUp("Fire1")) {
                weapon.BurstCamReset();
            }
            if (Input.GetKeyDown(KeyCode.F1)) {
                weapon.NextFireMode();
            }
            if (Input.GetButtonDown("Fire2")) {
                isAiming = !isAiming;
                weapon.Aim(isAiming);
            }

            if (Input.GetButton("Reload") && weapon.CanReload(nextFire)) {
                weapon.Reload();
            }
        }


    }
}