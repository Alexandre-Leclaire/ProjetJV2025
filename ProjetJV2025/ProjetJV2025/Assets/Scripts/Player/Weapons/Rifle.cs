using System.Collections;
using UnityEngine;

public class Rifle : Weapon
{
    public override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, weaponData.weaponRange,
                weaponData.targetLayerMask))
        {
            Debug.Log(hit.collider.name);
        }
    }
}