using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ShootParameters))]
public class ShootParametersInspector : Editor
{
    ShootParameters targetShootParameters;
    bool showProjectileParameters;

    private void OnEnable()
    {
        targetShootParameters = target as ShootParameters;
    }

    public override void OnInspectorGUI()
    {
        ShowShootParameters(targetShootParameters, 0, ref showProjectileParameters);
    }

    public static void ShowShootParameters(ShootParameters shootParameters, float indentValue, ref bool showProjectileParameters)
    {
        SerializedObject serializedParameters = new SerializedObject(shootParameters);

        serializedParameters.Update();

        #region Base Parameters
        GUI.Label(EditorStaticMethods.GetIndentedControlRect(indentValue), "Base Parameters", EditorStyles.boldLabel);

        SerializedProperty projectilePrefabAttribute = serializedParameters.FindProperty("projectilePrefab");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), projectilePrefabAttribute);

        SerializedProperty cadenceProperty = serializedParameters.FindProperty("shootCadence");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), cadenceProperty);

        SerializedProperty numberOfProjectilesPerShotProperty = serializedParameters.FindProperty("numberOfProjectilesPerShot");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), numberOfProjectilesPerShotProperty);
        
        SerializedProperty imprecisionAngleProperty = serializedParameters.FindProperty("imprecisionAngle");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), imprecisionAngleProperty);
        #endregion

        #region Serial Shots
        GUILayout.Space(20);
        GUI.Label(EditorStaticMethods.GetIndentedControlRect(indentValue), "Serial Shots", EditorStyles.boldLabel);

        SerializedProperty numberOfSerialShotsProperty = serializedParameters.FindProperty("numberOfSerialShots");
        bool isSerialShot = EditorGUI.Toggle(EditorStaticMethods.GetIndentedControlRect(indentValue), "Is serial Shot", numberOfSerialShotsProperty.intValue > 1);

        if (isSerialShot)
        {
            if (numberOfSerialShotsProperty.intValue < 2)
                numberOfSerialShotsProperty.intValue = 3;

            EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), numberOfSerialShotsProperty);

            SerializedProperty timeBetweenEachSerialShotProperty = serializedParameters.FindProperty("timeBetweenEachSerialShot");
            EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), timeBetweenEachSerialShotProperty);
        }
        else
        {
            if (numberOfSerialShotsProperty.intValue != 1)
                numberOfSerialShotsProperty.intValue = 1;
        }
        #endregion

        #region Projectile Parameters
        GUILayout.Space(20);

        GUI.Label(EditorStaticMethods.GetIndentedControlRect(indentValue), "Projectiles Parameters", EditorStyles.boldLabel);

        SerializedProperty projParamsAttribute = serializedParameters.FindProperty("projectileParameters");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), projParamsAttribute);

        serializedParameters.ApplyModifiedProperties();

        ProjectileParameters projParams = shootParameters.GetProjectileParameters;
        if (projParams != null)
        {
            showProjectileParameters = EditorGUI.BeginFoldoutHeaderGroup(EditorStaticMethods.GetIndentedControlRect(indentValue), showProjectileParameters, (showProjectileParameters ? "Close" : "Open") + " Projectile Parameters", showProjectileParameters ? EditorStyles.boldLabel : null);
            EditorGUI.EndFoldoutHeaderGroup();

            if (showProjectileParameters)
            {
                EditorGUILayout.BeginVertical("box");
                ProjectileParametersInspector.ShowProjectileParameters(projParams, indentValue + 15);
                EditorGUILayout.EndVertical();
            }
        }
        #endregion
    }
}
