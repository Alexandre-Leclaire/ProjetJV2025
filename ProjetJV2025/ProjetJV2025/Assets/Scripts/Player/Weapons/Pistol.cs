using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    
    [SerializeField] 
    private GameObject pointOfFire;

    public override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Fire1"))
        {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }
    
   public override void Shoot()
   {
     RaycastHit hit;
     if (Physics.Raycast(pointOfFire.transform.position, pointOfFire.transform.TransformDirection(Vector3.forward),
             out hit, weaponData.weaponRange, weaponData.targetLayerMask))
     {
         Debug.Log(hit.transform.name);
     }
   }
}
