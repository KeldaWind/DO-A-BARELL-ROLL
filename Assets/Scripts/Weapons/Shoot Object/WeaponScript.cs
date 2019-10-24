using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] ShootOriginScript[] shootOrigins = new ShootOriginScript[0];
    public void SetShootOrigins(List<ShootOriginScript> origins)
    {
        shootOrigins = origins.ToArray();
    }
    public ShootOriginScript[] GetShootOrigins { get { return shootOrigins; } }

    public void Shoot(ShootParameters shootParameters, DamageTag damageTag)
    {
        foreach (ShootOriginScript origin in shootOrigins)
            origin.Shoot(shootParameters, damageTag);
    }
}
