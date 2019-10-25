using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ShootOriginScript))]
public class ShootOriginInspector : Editor
{
    ShootOriginScript targetOrigin;

    private void OnEnable()
    {
        targetOrigin = target as ShootOriginScript;
    }

    private void OnSceneGUI()
    {
        ShowShootOrigin(targetOrigin);
    }

    public static void ShowShootOrigin(ShootOriginScript shootOrigin)
    {
        Vector3 pos = shootOrigin.transform.position;
        Quaternion rot = shootOrigin.GetShootRotation;
        Handles.SphereHandleCap(0, pos, rot, HandleUtility.GetHandleSize(shootOrigin.transform.position) * 0.2f, EventType.Repaint);
        Handles.ArrowHandleCap(0, pos, rot, HandleUtility.GetHandleSize(shootOrigin.transform.position) * 1f, EventType.Repaint);
    }
}
