using UnityEngine;

[CreateAssetMenu (fileName="NewGunData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public LayerMask targetLayerMask;
    
    [Header("FireConfig")]
    public float weaponDamage;
    public float weaponRange;
    public float rateOfFire;
    [Header("ReloadConfig")]
    public float magazineSize;
    public float reloadTime;
    
}
