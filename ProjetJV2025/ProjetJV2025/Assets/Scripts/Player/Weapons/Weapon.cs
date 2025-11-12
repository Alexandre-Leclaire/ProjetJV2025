using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public PlayerMovement playerMovement;
    public Transform cameraTransform;
    private float currentAmmo = 0f;
    private float nextTimetoFire = 0f;
    private bool isReloading = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = weaponData.magazineSize;
        playerMovement = transform.root.GetComponent<PlayerMovement>();
        cameraTransform = transform.root.GetComponentInChildren<Camera>().transform; // A changer on va tirer un raycast vers la position de la souris et non depuis la camera
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
        else if (Time.time >= nextTimetoFire)
        {
            nextTimetoFire = Time.time + (1 / weaponData.rateOfFire);
            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        currentAmmo--;
        Debug.Log("Current Ammo: " + currentAmmo);
        Shoot();
    }

    public abstract void Shoot();

}
