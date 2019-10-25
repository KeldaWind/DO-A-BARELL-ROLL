using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PoolingManager))]
public class PoolingManagerInspector : Editor
{
    PoolingManager targetPoolManager;

    private void OnEnable()
    {
        targetPoolManager = target as PoolingManager;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(8);

        if (GUILayout.Button("Clear Pooling System"))
            targetPoolManager.Clear();
        GUILayout.Space(8);

        if (GUILayout.Button("Assign Pool Values with Library"))
            targetPoolManager.AssignPoolValuesWithLibrary();
        GUILayout.Space(8);

        if (GUILayout.Button("Generate Pool Objects"))
            targetPoolManager.GeneratePoolsObjects();

        GUILayout.Space(8);
        
        base.OnInspectorGUI();
    }
}
