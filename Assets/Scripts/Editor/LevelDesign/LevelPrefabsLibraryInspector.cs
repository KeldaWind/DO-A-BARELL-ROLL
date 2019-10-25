using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelPrefabsLibrary))]
public class LevelPrefabsLibraryInspector : Editor
{
    LevelPrefabsLibrary targetLibrary;

    private void OnEnable()
    {
        targetLibrary = target as LevelPrefabsLibrary;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(16);
    }

    void DebugLibraryElement(object index)
    {
        LevelPrefabsLibrary.DebugPrefabInformations(targetLibrary.GetEnemyPrefabInformations[(int)index]);
    }
}
