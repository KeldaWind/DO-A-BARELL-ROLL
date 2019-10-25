using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DO A BARELL ROLL/Scriptables/Weapons/Weapon Parameters", fileName = "NewWeaponParameters", order = 0)]
public class WeaponParameters : ScriptableObject
{
    public string weaponName = "";

    public void SetShootParameters(ShootParameters shootParams)
    {
        shootParameters = shootParams;
    }
    [SerializeField] ShootParameters shootParameters = default;
    public ShootParameters GetShootParameters { get { return shootParameters; } }

    public void SetWeaponPrefab(WeaponScript weapon)
    {
        weaponPrefab = weapon;
    }
    [SerializeField] WeaponScript weaponPrefab = default;
    public WeaponScript GetWeaponPrefab { get { return weaponPrefab; } }
}