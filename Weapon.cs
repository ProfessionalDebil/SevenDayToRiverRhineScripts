using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [NonSerialized]
    public Animator animator;
    
    public Values values;

    public Magazine magazine;

    // fire mode
    [NonSerialized]
    public int fireModeIndex;
    [NonSerialized]
    private bool isReloading;
    [NonSerialized]
    public int fireModeCap;
    [NonSerialized]
    public int shotsFired;

    public List<Weapon> subweapons;

    [NonSerialized]
    public Weapon parentWeapon;
    private readonly int ANIMATOR_PARAMETER_RELOAD = Animator.StringToHash("reload");
    private readonly int ANIMATOR_PARAMETER_FIRE = Animator.StringToHash("fire");
    private readonly int ANIMATOR_PARAMETER_FIREMODE = Animator.StringToHash("fireMode");
    private readonly int ANIMATOR_PARAMETER_AIM = Animator.StringToHash("aim");

    void Awake() {
        animator = GetComponent<Animator>();
        
        if (animator != null && animator.runtimeAnimatorController == null)
        {
            animator = null;
        }
        if (animator == null && parentWeapon != null)
        {
            animator = parentWeapon.GetComponent<Animator>();
        }

        values.fireRate = 60 / values.fireRate;

        fireModeCap = GetFireModeCap();

        InitializeSubWeapons();
    }

    void InitializeSubWeapons() {
        foreach (Weapon subWeapon in subweapons) {
            subWeapon.parentWeapon = this;
        }
    }

    public void Fire() {
        ProjectileManager.SpawnProjectile(this, values.muzzleTransform.position, values.muzzleTransform.rotation);
        shotsFired++;
        magazine.Unload(values.bulletPerShot);
        values.casingParticle.Emit(1);
        animator.SetTrigger(ANIMATOR_PARAMETER_FIRE);
    }

    public void Aim(bool isAiming) {
        Camera.main.fieldOfView = isAiming ? values.aimFov : 70;
        animator.SetBool(ANIMATOR_PARAMETER_AIM, isAiming);
    }

    public void NextFireMode() {
        fireModeIndex++;
        FireMode newMode = GetFireMode();
        fireModeCap = newMode.rounds;

        // TODO: make this affect animators
        animator.SetInteger(ANIMATOR_PARAMETER_FIREMODE, newMode.animatorValue);
    }

    public void BurstCamReset() {
        if (GetFireMode().autoReset || shotsFired == fireModeCap) {
            shotsFired = 0;
        }
    }

    public int GetFireModeCap() {
        return GetFireMode().rounds;
    }

    public FireMode GetFireMode() {
         return values.fireModes[fireModeIndex % values.fireModes.Count];
    }

    public bool CanFire(float nextFire) {
        bool flag1 = Time.time >= nextFire;
        bool flag2 = magazine.GetCapacity() > 0;
        bool flag3 = !(shotsFired == fireModeCap);

        return flag1 && flag2 && flag3;
    }

    public bool CanReload(float nextFire) {
        bool flag1 = Time.time >= nextFire;
        bool flag2 = !isReloading;

        return flag1 && flag2;
    }

    public void Reload() {
        isReloading = true;
        animator.SetTrigger(ANIMATOR_PARAMETER_RELOAD);
    }

    public void EndReload() {
        isReloading = false;
        Magazine newMagazine = InventorySlotManager.GetMagazineToReload(values.magType);
        InventorySlotManager.Take(newMagazine);
        InventorySlotManager.Put(magazine);
        magazine = newMagazine;
    }

    [Serializable]
    public class Values {
        public WeaponType weaponType;
        public float fireRate;
        public float aimFov = 60;
        public Transform muzzleTransform;
        public ParticleSystem muzzleFlash;
        public ParticleSystem casingParticle;
        public GameObject projectilePrefab;
        public List<FireMode> fireModes;
        public MagazineType magType;
        public int bulletPerShot = 1;
    }
    [Serializable]
    public class FireMode {
        public int rounds;
        public bool autoReset;
        public bool usesAnimator;
        public int animatorValue;
    }
}