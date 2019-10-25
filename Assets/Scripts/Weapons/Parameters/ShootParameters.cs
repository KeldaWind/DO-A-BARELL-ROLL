using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DO A BARELL ROLL Scriptables/Weapons/Shoot Parameters", fileName = "NewShootParameters", order = 0)]
public class ShootParameters : ScriptableObject
{
    [SerializeField] ProjectileScript projectilePrefab = default;
    public ProjectileScript GetProjectilePrefab { get { return projectilePrefab; } }
    public void SetProjectilePrefab(ProjectileScript projectile)
    {
        projectilePrefab = projectile;
    }

    [SerializeField] float shootCadence = 0.5f;
    public float GetShootCadence { get { return shootCadence; } }

    [SerializeField] int numberOfProjectilesPerShot = 1;
    public int GetNumberOfProjectilesPerShot { get { return numberOfProjectilesPerShot; } }

    [SerializeField] float imprecisionAngle = 0;
    public float GetImprecisionAngle { get { return imprecisionAngle; } }


    [SerializeField] int numberOfSerialShots = 1;
    public int GetNumberOfSerialShots { get { return numberOfSerialShots; } }

    [SerializeField] float timeBetweenEachSerialShot = 0.2f;
    public float GetTimeBetweenEachSerialShot { get { return timeBetweenEachSerialShot; } }

    public void SetProjectileParameters(ProjectileParameters projParams)
    {
        projectileParameters = projParams;
    }
    public ProjectileParameters GetProjectileParameters { get { return projectileParameters; } }

    [SerializeField] ProjectileParameters projectileParameters;
}
