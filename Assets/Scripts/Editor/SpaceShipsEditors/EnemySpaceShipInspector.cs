using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpaceShipScript))]
public class EnemySpaceShipInspector : SpaceShipInspector
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(8);

        serializedObject.Update();

        GUILayout.Label("Enemy Parameters", EditorStyles.boldLabel);

        SerializedProperty aimingTypeProperty = serializedObject.FindProperty("aimingType");
        if (aimingTypeProperty != null)
            EditorGUILayout.PropertyField(aimingTypeProperty);

        serializedObject.ApplyModifiedProperties();
    }
}
