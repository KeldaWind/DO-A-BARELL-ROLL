using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpaceShipMovementParameters))]
public class SpaceShipMovementParametersInspector : Editor
{
    SpaceShipMovementParameters targetParameters;
    private void OnEnable()
    {
        targetParameters = target as SpaceShipMovementParameters;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        ShowMovementParameters(targetParameters, 0);
    }

    public static void ShowMovementParameters(SpaceShipMovementParameters movementParameters, float indentValue)
    {
        SerializedObject serializedObject = new SerializedObject(movementParameters);

        serializedObject.Update();

        SerializedProperty baseMovementValues = serializedObject.FindProperty("baseMovementValues");

        EditorGUILayout.PropertyField(baseMovementValues);

        SerializedProperty barellRollValues = serializedObject.FindProperty("barellRollValues");
        EditorGUILayout.PropertyField(barellRollValues);

        serializedObject.ApplyModifiedProperties();
    }
}
