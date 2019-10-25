using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(WeaponParameters))]
public class WeaponParametersInspector : Editor
{
    WeaponParameters targetWeaponParameters;
    bool showShootParameters;
    bool showProjectileParameters;

    private void OnEnable()
    {
        targetWeaponParameters = target as WeaponParameters;
    }

    public override void OnInspectorGUI()
    {
        ShowWeaponParameters(targetWeaponParameters, 0, ref showShootParameters, ref showProjectileParameters);
    }

    public static void ShowWeaponParameters(WeaponParameters weaponParameters, float indentValue, ref bool showShootParameters, ref bool showProjectileParameters)
    {
        SerializedObject serializedParameters = new SerializedObject(weaponParameters);

        serializedParameters.Update();

        #region Base Parameters
        GUI.Label(EditorStaticMethods.GetIndentedControlRect(indentValue), "Base Parameters", EditorStyles.boldLabel);

        SerializedProperty weaponNameAttribute = serializedParameters.FindProperty("weaponName");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), weaponNameAttribute);

        SerializedProperty weaponPrefabAttribute = serializedParameters.FindProperty("weaponPrefab");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), weaponPrefabAttribute);
        #endregion

        #region Shoot Parameters
        GUILayout.Space(20);

        GUI.Label(EditorStaticMethods.GetIndentedControlRect(indentValue), "Shoot Parameters", EditorStyles.boldLabel);

        SerializedProperty shootParamsAttribute = serializedParameters.FindProperty("shootParameters");
        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), shootParamsAttribute);

        serializedParameters.ApplyModifiedProperties();

        ShootParameters shootParams = weaponParameters.GetShootParameters;
        if (shootParams != null)
        {
            showShootParameters = EditorGUI.BeginFoldoutHeaderGroup(EditorStaticMethods.GetIndentedControlRect(indentValue), showShootParameters, (showShootParameters ? "Close" : "Open") + " Shoot Parameters", showShootParameters ? EditorStyles.boldLabel : null);
            EditorGUI.EndFoldoutHeaderGroup();

            if (showShootParameters)
            {
                EditorGUILayout.BeginVertical("box");
                ShootParametersInspector.ShowShootParameters(shootParams, indentValue + 15, ref showProjectileParameters);
                EditorGUILayout.EndVertical();
            }
        }
        #endregion
    }
}
