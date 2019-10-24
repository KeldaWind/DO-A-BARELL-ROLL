using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOriginScript : MonoBehaviour
{
    [SerializeField] Vector3 baseShootDirection = Vector3.up;
    public Vector3 GetTrueDirection { get { return transform.rotation * baseShootDirection; } }

    public Quaternion GetShootRotation
    {
        get
        {
            float rotZ = Mathf.Atan2(GetTrueDirection.y, GetTrueDirection.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(new Vector3(rotZ + 180, -90, 0));
        }
    }

    public void Shoot(ShootParameters shootParameters, DamageTag damageTag)
    {
        ProjectileScript projPrefab = shootParameters.GetProjectilePrefab;
        float imprecisionAngle = shootParameters.GetImprecisionAngle;
        for (int i = 0; i < shootParameters.GetNumberOfProjectilesPerShot; i++)
        {
            ProjectileScript newProj = Instantiate(projPrefab, transform.position, Quaternion.identity);
            Vector3 shootDirection = Quaternion.Euler(0, 0, Random.Range(-imprecisionAngle/2, imprecisionAngle/2)) * GetTrueDirection;
            newProj.Shoot(shootParameters.GetProjectileParameters, shootDirection, damageTag);
        }
    }
}
