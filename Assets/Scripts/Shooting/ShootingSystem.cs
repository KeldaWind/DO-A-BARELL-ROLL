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

    [SerializeField] WeaponParameters baseWeaponParameters = default;
    public WeaponParameters GetBaseWeaponParameters { get { return baseWeaponParameters; } }
    public void SetWeaponParameters(WeaponParameters parameters)
    {
        baseWeaponParameters = parameters;
    }

    ShootParameters currentShootParameters;
    [HideInInspector] [SerializeField] WeaponScript currentWeapon;

    TimerSystem cadenceTimerSystem;
    TimerSystem serialShotTimerSystem;

    public void SetUp(DamageTag dmgTag, bool instantiateWeapon)
    {
        currentShootParameters = baseWeaponParameters.GetShootParameters;
        cadenceTimerSystem = new TimerSystem(currentShootParameters.GetShootCadence, null);

        serialShotTimerSystem = new TimerSystem(currentShootParameters.GetTimeBetweenEachSerialShot, EndShooting, currentShootParameters.GetNumberOfSerialShots - 1, Shoot);

        if (instantiateWeapon)
            InstantiateBaseWeapon();

        damageTag = dmgTag;
    }

    bool baseWeaponInstantiated;
    public void InstantiateBaseWeapon()
    {
        if (baseWeaponInstantiated)
            return;

        baseWeaponInstantiated = true;

        if (baseWeaponParameters != null)
        {
            if (baseWeaponParameters.GetWeaponPrefab != null)
            {
                WeaponScript newWeaponObject = Object.Instantiate(baseWeaponParameters.GetWeaponPrefab, weaponsParent);
                WeaponSet baseWeaponSet = new WeaponSet(newWeaponObject, baseWeaponParameters, baseWeaponParameters.GetShootParameters);
                allWeaponSets.Add(baseWeaponSet);
            }
        }

        SelectWeapon();
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
        if (currentShootParameters.GetNumberOfSerialShots > 1)
            serialShotTimerSystem.StartTimer();
        else
            EndShooting();
    }

    public void Shoot()
    {
        currentWeapon.Shoot(currentShootParameters, damageTag);
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

    #region Multiple Weapons
    List<WeaponSet> allWeaponSets = new List<WeaponSet>();
    public void SetUpWeaponSets(List<WeaponParameters> allOtherWeapons)
    {
        if (!baseWeaponInstantiated)
        {
            baseWeaponInstantiated = true;
            if (baseWeaponParameters != null)
            {
                if (baseWeaponParameters.GetWeaponPrefab != null)
                {
                    WeaponScript newWeaponObject = Object.Instantiate(baseWeaponParameters.GetWeaponPrefab, weaponsParent);
                    WeaponSet baseWeaponSet = new WeaponSet(newWeaponObject, baseWeaponParameters, baseWeaponParameters.GetShootParameters);
                    allWeaponSets.Add(baseWeaponSet);
                }
            }
        }

        foreach(WeaponParameters otherWeapon in allOtherWeapons)
        {
            if (otherWeapon != null)
            {
                if (otherWeapon.GetWeaponPrefab != null)
                {
                    WeaponScript newWeaponObject = Object.Instantiate(otherWeapon.GetWeaponPrefab, weaponsParent);
                    WeaponSet otherWeaponSet = new WeaponSet(newWeaponObject, otherWeapon, otherWeapon.GetShootParameters);
                    allWeaponSets.Add(otherWeaponSet);
                }
            }
        }

        SelectWeapon();
    }

    int currentWeaponIndex;

    public WeaponScript GetSelectedWeaponObject { get { return allWeaponSets.Count > 0 ? allWeaponSets[currentWeaponIndex].weaponObject : null; } }
    public WeaponParameters GetSelectedWeaponParameters { get { return allWeaponSets.Count > 0 ? allWeaponSets[currentWeaponIndex].weaponParameters : null; } }
    public ShootParameters GetSelectedShootParameters{ get { return allWeaponSets.Count > 0 ? allWeaponSets[currentWeaponIndex].shootParameters : null; } }

    public void NextWeapon()
    {
        if (IsShooting || allWeaponSets.Count == 0)
            return;

        currentWeaponIndex++;
        if (currentWeaponIndex >= allWeaponSets.Count)
            currentWeaponIndex = 0;

        SelectWeapon();
    }

    public void PreviousWeapon()
    {
        if (IsShooting || allWeaponSets.Count == 0)
            return;

        currentWeaponIndex--;
        if (currentWeaponIndex < 0)
            currentWeaponIndex = allWeaponSets.Count - 1;

        SelectWeapon();
    }

    public void SelectWeapon()
    {
        currentWeapon = GetSelectedWeaponObject;
        currentShootParameters = GetSelectedShootParameters;

        Debug.Log(GetSelectedWeaponParameters.weaponName);

        if (cadenceTimerSystem != null)
            cadenceTimerSystem.ChangeTimerValue(currentShootParameters.GetShootCadence);

        if (cadenceTimerSystem != null)
        {
            serialShotTimerSystem.ChangeTimerValue(currentShootParameters.GetTimeBetweenEachSerialShot);
            serialShotTimerSystem.ChangeIterationValue(currentShootParameters.GetNumberOfSerialShots - 1);
        }

        OnWeaponChanged?.Invoke(GetSelectedWeaponParameters.weaponName);
    }

    public System.Action<string> OnWeaponChanged;
    #endregion
}

[System.Serializable]
public struct WeaponSet
{
    public WeaponSet(WeaponScript weaponObj, WeaponParameters weaponParams, ShootParameters shootParams)
    {
        weaponObject = weaponObj;
        weaponParameters = weaponParams;
        shootParameters = shootParams;
    }

    public WeaponScript weaponObject;
    public WeaponParameters weaponParameters;
    public ShootParameters shootParameters;
}