using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ProjectileParameters))]
public class ProjectileParametersInspector : Editor
{
    ProjectileParameters targetProjectileParameters;

    private void OnEnable()
    {
        targetProjectileParameters = target as ProjectileParameters;
    }

    public override void OnInspectorGUI()
    {
        ShowProjectileParameters(targetProjectileParameters, 0);
    }

    public static void ShowProjectileParameters(ProjectileParameters projParameters, float indentValue)
    {
        SerializedObject serializedParameters = new SerializedObject(projParameters);

        serializedParameters.Update();

        SerializedProperty speedAttribute = serializedParameters.FindProperty("projectileSpeed");
        SerializedProperty lifetimeAttribute = serializedParameters.FindProperty("projectileLifetime");
        SerializedProperty sizeAttribute = serializedParameters.FindProperty("projectileSize");
        SerializedProperty damagesAttribute = serializedParameters.FindProperty("projectileDamages");

        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), speedAttribute);

        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), lifetimeAttribute);

        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), sizeAttribute);

        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), damagesAttribute);

        serializedParameters.ApplyModifiedProperties();
    }
}
