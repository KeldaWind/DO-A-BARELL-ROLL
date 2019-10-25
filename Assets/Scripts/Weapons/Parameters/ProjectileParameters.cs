using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DO A BARELL ROLL Scriptables/Weapons/Projectile Parameters", fileName = "NewProjectileParameters", order = 0)]
public class ProjectileParameters : ScriptableObject
{
    public MinMaxFloat GetProjectileSpeed { get { return projectileSpeed; } }
    [SerializeField] MinMaxFloat projectileSpeed = new MinMaxFloat(20, 20);

    public MinMaxFloat GetProjectileLifetime { get { return projectileLifetime; } }
    [SerializeField] MinMaxFloat projectileLifetime = new MinMaxFloat(1f, 1f);

    public float GetProjectileSize { get { return projectileSize; } }
    [SerializeField] float projectileSize = 0.2f;

    public int GetProjectileDamages { get { return projectileDamages; } }
    [SerializeField] int projectileDamages = 10;
}
