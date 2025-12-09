using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] 
    private Weapon[] weapons;
    public GameObject player;

    [SerializeField]
    private AvatarMask _upperBodyMask;

    [SerializeField] 
    private TMP_Text _weaponNameText;
    [SerializeField] 
    private TMP_Text _ammoText;
    [SerializeField] 
    private Slider _ammoSlider;
    
    [SerializeField]
    private Rig _rifleAim; 
    [SerializeField]
    private Rig _rifleBodyAim; 
    [SerializeField] 
    private Rig _pistolAim; 
    [SerializeField] 
    private Rig _pistolBodyAim; 
    [SerializeField] 
    private GameObject _parentRifleRigs;
    [SerializeField]
    private GameObject _parentPistolRigs;

    private Rig _rigAim;
    private Rig _rigBodyAim;
    private Rig[] rifleRigs;
    private Rig[] pistolRigs;
    
    private int _selectedWeapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapons = player.GetComponentsInChildren<Weapon>();
        if (weapons.Length > 0)
        {
            _selectedWeapon = 0;
            for (int i = 1; i < weapons.Length; i++)
            {
                weapons[i].gameObject.SetActive(false);
            }
            
            rifleRigs = _parentRifleRigs.GetComponentsInChildren<Rig>();
            pistolRigs = _parentPistolRigs.GetComponentsInChildren<Rig>();
            
            _rifleBodyAim.weight = 0;
            _pistolBodyAim.weight = 0;
            
            SelectWeapon(); 
        }
        else
        {
            _selectedWeapon = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int tmp = _selectedWeapon;
        SwitchWeapons();
        if (_selectedWeapon != tmp)
        {
            SelectWeapon(); 
        }

        _ammoSlider.value = ((float)weapons[_selectedWeapon].currentAmmo) /
                            ((float)weapons[_selectedWeapon].weaponData.magazineSize);
        _ammoText.text = $"{weapons[_selectedWeapon].currentAmmo}/{weapons[_selectedWeapon].weaponData.magazineSize}";
        if (weapons[_selectedWeapon].currentAmmo == 0)
        {
            _ammoText.text += "(R)";
        }
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Aim();
        }
        else
        {
            UnAim();
        }
    }
   
    private void SwitchWeapons()
    {
        if (weapons.Length == 1)
        {
            return;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            weapons[_selectedWeapon].gameObject.SetActive(false);
            _selectedWeapon += 1;
            if (_selectedWeapon > weapons.Length - 1)
            { 
                _selectedWeapon = 0;
            }
            weapons[_selectedWeapon].gameObject.SetActive(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            weapons[_selectedWeapon].gameObject.SetActive(false);
            _selectedWeapon -= 1;
            if (_selectedWeapon < 0)
            {
                _selectedWeapon = weapons.Length - 1;
            } 
            weapons[_selectedWeapon].gameObject.SetActive(true);
        }
    }

    #region utilities

        
    private void Aim()
    {
        weapons[_selectedWeapon].laser.SetActive(true);
        _rigAim.weight += Time.deltaTime / 0.3f;
        _rigBodyAim.weight += Time.deltaTime / 0.3f;
    }

    private void UnAim()
    {
        weapons[_selectedWeapon].laser.SetActive(false);
        _rigAim.weight -= Time.deltaTime / 0.3f;
        _rigBodyAim.weight -= Time.deltaTime / 0.3f;
    }

    private void ActivateLayer(Rig[] rigs, Rig aimRig, Rig aimBodyRig)
    {
        foreach (var rig in rigs)
        {
            rig.weight = 1;
        }
        _rigAim = aimRig;
        _rigBodyAim = aimBodyRig;
    }

    private void DeactivateLayer(Rig[] rigs)
    {
        foreach (var rig in rigs)
        {
            rig.weight = 0;
        }
    }

    private void SelectWeapon()
    {
        _weaponNameText.text = weapons[_selectedWeapon].weaponData.weaponName;
        
        if (weapons[_selectedWeapon] is Rifle)
        {
            ActivateLayer(rifleRigs, _rifleAim, _rifleBodyAim);
            DeactivateLayer(pistolRigs);
                
            _upperBodyMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftArm, false);
            _upperBodyMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftFingers, false);
            _upperBodyMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftHandIK, false);
        }
        else if (weapons[_selectedWeapon] is Pistol)
        {
            ActivateLayer(pistolRigs, _pistolAim, _pistolBodyAim);
            DeactivateLayer(rifleRigs);
                
            _upperBodyMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftArm, true);
            _upperBodyMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftFingers, true);
            _upperBodyMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftHandIK, true);
        }
    }

    #endregion
}
