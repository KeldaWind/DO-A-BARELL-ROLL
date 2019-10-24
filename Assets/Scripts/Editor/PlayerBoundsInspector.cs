using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerBounds))]
public class PlayerBoundsInspector : Editor
{
    PlayerBounds targetBounds;

    private void OnEnable()
    {
        targetBounds = target as PlayerBounds;
    }

    private void OnSceneGUI()
    {

        targetBounds.downLeftLimit = Handles.FreeMoveHandle(targetBounds.downLeftLimit, Quaternion.identity, HandleUtility.GetHandleSize(targetBounds.transform.position) * 0.2f, Vector3.one * 0.1f, Handles.CubeHandleCap);
        targetBounds.downLeftLimit.z = 0;

        targetBounds.upRightLimit = Handles.FreeMoveHandle(targetBounds.upRightLimit, Quaternion.identity, HandleUtility.GetHandleSize(targetBounds.transform.position) * 0.2f, Vector3.one * 0.1f, Handles.CubeHandleCap);
        targetBounds.upRightLimit.z = 0;

    }
}