using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DO A BARELL ROLL/Scriptables/Weapons/Projectile Parameters", fileName = "NewProjectileParameters", order = 0)]
public class ProjectileParameters : ScriptableObject
{
    public MinMaxFloat GetProjectileSpeed { get { return projectileSpeed; } }
    [SerializeField] MinMaxFloat projectileSpeed = new MinMaxFloat(20, 20);
    public void ChangeProjectileMinSpeed(float newValue) { projectileSpeed.minValue = newValue; }
    public void ChangeProjectileMaxSpeed(float newValue) { projectileSpeed.maxValue = newValue; }

    public MinMaxFloat GetProjectileLifetime { get { return projectileLifetime; } }
    [SerializeField] MinMaxFloat projectileLifetime = new MinMaxFloat(1f, 1f);
    public void ChangeProjectileMinLifetime(float newValue) { projectileLifetime.minValue = newValue; }
    public void ChangeProjectileMaxLifetime(float newValue) { projectileLifetime.maxValue = newValue; }

    public float GetProjectileSize { get { return projectileSize; } }
    [SerializeField] float projectileSize = 0.2f;
    public void ChangeSize(float newSize) { projectileSize = newSize; }

    public int GetProjectileDamages { get { return projectileDamages; } }
    [SerializeField] int projectileDamages = 10;
    public void ChangeDamages(int newDamages) { projectileDamages = newDamages; }
}
