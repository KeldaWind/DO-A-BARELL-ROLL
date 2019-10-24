using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootingSystem
{
    [SerializeField] Transform weaponsParent = default;
    DamageTag damageTag;
    public Transform GetWeaponsParent { get { return weaponsParent; } }
    public void SetWeaponsParent(Transform parent)
    {
        weaponsParent = parent;
    }

    [SerializeField] WeaponParameters weaponParameters = default;
    public WeaponParameters GetWeaponParameters { get { return weaponParameters; } }
    public void SetWeaponParameters(WeaponParameters parameters)
    {
        weaponParameters = parameters;
    }

    ShootParameters shootParameters;
    [HideInInspector] [SerializeField] WeaponScript weapon;

    TimerSystem cadenceTimerSystem;
    TimerSystem serialShotTimerSystem;

    public void SetUp(DamageTag dmgTag, bool instantiateWeapon)
    {
        shootParameters = weaponParameters.GetShootParameters;
        cadenceTimerSystem = new TimerSystem(shootParameters.GetShootCadence, null);

        serialShotTimerSystem = new TimerSystem(shootParameters.GetTimeBetweenEachSerialShot, EndShooting, shootParameters.GetNumberOfSerialShots - 1, Shoot);

        if (instantiateWeapon)
            InstantiateWeapon();

        damageTag = dmgTag;
    }

    public void InstantiateWeapon()
    {
        weapon = Object.Instantiate(weaponParameters.GetWeaponPrefab, weaponsParent);
    }

    public void Reset()
    {
        cadenceTimerSystem.EndTimer();
        serialShotTimerSystem.EndTimer();
    }

    public void OnInputPressed()
    {
        if (CanShoot)
            StartShooting();
    }

    public void OnInputMaintained()
    {
        if (CanShoot)
            StartShooting();
    }

    public void OnInputReleased()
    {

    }

    public void StartShooting()
    {
        Shoot();
        if (shootParameters.GetNumberOfSerialShots > 1)
            serialShotTimerSystem.StartTimer();
        else
            EndShooting();
    }

    public void Shoot()
    {
        weapon.Shoot(shootParameters, damageTag);
    }

    public void UpdateShooting()
    {
        if (!serialShotTimerSystem.TimerOver)
            serialShotTimerSystem.UpdateTimer();
    }

    public void EndShooting()
    {
        cadenceTimerSystem.StartTimer();
    }

    public void UpdateSystem()
    {
        if (IsShooting)
            UpdateShooting();
        else if (!CanShoot)
            cadenceTimerSystem.UpdateTimer();
    }

    public bool CanShoot { get { return !IsShooting && cadenceTimerSystem.TimerOver; } }
    public bool IsShooting { get { return !serialShotTimerSystem.TimerOver; } }
}
