using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    
    [SerializeField] 
    private GameObject pointOfFire;

    public override void Update()
    {
        base.Update();
        if (Input.GetButton("Fire1"))
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
     
     var tracer = Instantiate(tracerEffect, pointOfFire.transform.position, Quaternion.identity);
     tracer.AddPosition(pointOfFire.transform.position);
     if (Physics.Raycast(pointOfFire.transform.position, pointOfFire.transform.TransformDirection(Vector3.forward),
             out hit, weaponData.weaponRange, weaponData.targetLayerMask))
     {
         //Debug.Log(hit.transform.name);
         hitEffect.transform.position = hit.point;
         hitEffect.transform.forward = hit.normal;
         hitEffect.Emit(1);
         
         tracer.transform.position = hit.point;
         if (hit.collider.gameObject.TryGetComponent(out IDamageable damageable))
         {
             damageable.TakeDamage((int) weaponData.weaponDamage);
         }
     }
     else
     {
         tracer.transform.position = gunLookAt.position;
     }
   }
}
