using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    
    private float currentAmmo = 0f;
    private float nextTimetoFire = 0f;
    private bool isReloading = false;
    
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;

    public Transform gunLookAt;
    public GameObject laser;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = weaponData.magazineSize;
    }

    public virtual void Update()
    {
        
    }

    public void TryReload()
    {
        if (!isReloading && currentAmmo < weaponData.magazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(weaponData.reloadTime);
        
        currentAmmo = weaponData.magazineSize;
        isReloading = false;
        
        Debug.Log("Reload complete");
    }

    public void TryShoot()
    {
        if (isReloading || currentAmmo <= 0f)
        {
            return;
        }
        else if (Time.time >= nextTimetoFire && Input.GetMouseButton(1))
        {
            nextTimetoFire = Time.time + (1 / weaponData.rateOfFire);
            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        currentAmmo--;
        muzzleFlash.Emit(1);
        Debug.Log("Current Ammo: " + currentAmmo);
        Shoot();
    }

    public abstract void Shoot();

}
