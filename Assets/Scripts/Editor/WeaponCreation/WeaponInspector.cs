using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponScript))]
public class WeaponInspector : Editor
{
    WeaponScript targetweapon;

    private void OnEnable()
    {
        targetweapon = target as WeaponScript;
    }

    private void OnSceneGUI()
    {
        foreach(ShootOriginScript origin in targetweapon.GetShootOrigins)
            ShootOriginInspector.ShowShootOrigin(origin);
    }
}
