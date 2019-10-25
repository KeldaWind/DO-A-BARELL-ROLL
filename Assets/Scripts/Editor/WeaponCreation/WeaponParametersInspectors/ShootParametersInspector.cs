using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), cadenceProperty, new GUIContent("Shoot Cadence", "The reloading time needed after a shot to be able to shoot again"));
        if (cadenceProperty.floatValue < 0)
            cadenceProperty.floatValue = 0;

        SerializedProperty numberOfProjectilesPerShotProperty = serializedParameters.FindProperty("numberOfProjectilesPerShot");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), numberOfProjectilesPerShotProperty, new GUIContent("Number of Projectiles per Shot", "The number of projectile that will be instantiated on shot by each shoot origins of the weapon. Generaly of one except for shotguns"));
        if (numberOfProjectilesPerShotProperty.intValue < 1)
            numberOfProjectilesPerShotProperty.intValue = 1;

        SerializedProperty imprecisionAngleProperty = serializedParameters.FindProperty("imprecisionAngle");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), imprecisionAngleProperty, new GUIContent("Imprecision Angle", "The angle of the cone in which the projectile direction will be randomly picked on shot. Set low for precise shots, set high for spray"));
        if (imprecisionAngleProperty.floatValue < 0)
            imprecisionAngleProperty.floatValue = 0;
        #endregion

        #region Serial Shots
        GUILayout.Space(20);
        GUI.Label(EditorStaticMethods.GetIndentedControlRect(indentValue), "Serial Shots", EditorStyles.boldLabel);

        SerializedProperty numberOfSerialShotsProperty = serializedParameters.FindProperty("numberOfSerialShots");
        bool isSerialShot = EditorGUI.Toggle(EditorStaticMethods.GetIndentedControlRect(indentValue), new GUIContent("Is Serial Shots", "Set true if you want a burst shot"), numberOfSerialShotsProperty.intValue > 1);

        if (isSerialShot)
        {
            if (numberOfSerialShotsProperty.intValue < 2)
                numberOfSerialShotsProperty.intValue = 3;

            EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), numberOfSerialShotsProperty, new GUIContent("Number of Shots", "The number of shots that will be done through the burst shot"));

            SerializedProperty timeBetweenEachSerialShotProperty = serializedParameters.FindProperty("timeBetweenEachSerialShot");
            EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), timeBetweenEachSerialShotProperty, new GUIContent("Time between each Shot", "The time between two shots of the burst shot"));

            if (numberOfSerialShotsProperty.intValue < 2)
                numberOfSerialShotsProperty.intValue = 2;

            if (timeBetweenEachSerialShotProperty.floatValue < 0)
                timeBetweenEachSerialShotProperty.floatValue = 0;
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
