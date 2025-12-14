using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Data")]
    public WeaponData weaponData;

    [Header("Runtime")]
    public int currentAmmo;

    float nextTimeToFire;
    bool isReloading;

    [Header("FX")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;

    [Header("Visuals")]
    public Transform gunLookAt;
    public GameObject laser;

    protected Inventory inventory;

    void Start()
    {
        inventory = GetComponentInParent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError($"[Weapon] No Inventory found in parent for {name}");
        }

        currentAmmo = weaponData.magazineSize;
    }

    public virtual void Update()
    {

    }

    public void TryShoot()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            TryReload();
            return;
        }

        if (Time.time < nextTimeToFire)
            return;

        if (!Input.GetMouseButton(1))
            return;

        nextTimeToFire = Time.time + (1f / weaponData.rateOfFire);
        ShootInternal();
    }

    void ShootInternal()
    {
        currentAmmo--;

        if (muzzleFlash != null)
            muzzleFlash.Emit(1);

        Shoot();
    }

    public void TryReload()
    {
        if (isReloading)
            return;

        if (currentAmmo >= weaponData.magazineSize)
            return;

        if (inventory == null || !inventory.Has("Ammo", 1))
        {
            Debug.Log("[Weapon] No ammo in inventory");
            return;
        }

        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("[Weapon] Reloading...");

        yield return new WaitForSeconds(weaponData.reloadTime);

        int ammoNeeded = weaponData.magazineSize - currentAmmo;
        int ammoAvailable = inventory.GetQuantity("Ammo");
        int ammoToLoad = Mathf.Min(ammoNeeded, ammoAvailable);

        if (ammoToLoad > 0)
        {
            inventory.Remove("Ammo", ammoToLoad);
            currentAmmo += ammoToLoad;
        }

        isReloading = false;

        Debug.Log($"[Weapon] Reload complete ({currentAmmo}/{weaponData.magazineSize})");
    }

    public abstract void Shoot();
}
