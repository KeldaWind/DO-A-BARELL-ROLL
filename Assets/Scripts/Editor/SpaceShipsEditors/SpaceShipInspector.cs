using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SpaceShipScript))]
public class SpaceShipInspector : Editor
{
    SpaceShipScript targetSpaceShip;
    bool showMovementParameters;

    bool showWeaponParameters;
    bool showShootParameters;
    bool showProjectileParameters;

    public virtual void OnEnable()
    {
        targetSpaceShip = target as SpaceShipScript;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();


        System.Type shipType = targetSpaceShip.GetType();
        if (shipType != typeof(PlayerSpaceShipScript))
        {
            GUILayout.Space(8);

            serializedObject.Update();
            SerializedProperty damageTagProperty = serializedObject.FindProperty("damageTag");
            EditorGUILayout.PropertyField(damageTagProperty);

            serializedObject.ApplyModifiedProperties();
        }

        #region Movements
        GUILayout.Space(8);
        EditorGUILayout.LabelField("Movement System", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        SpaceShipMovementParameters newMovementParameters =
            (SpaceShipMovementParameters)EditorGUILayout.ObjectField
            ("Movement Parameters", targetSpaceShip.GetMovementSystem.GetMovementParameters, typeof(SpaceShipMovementParameters), false);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetSpaceShip, "Undo Affect Movement Parameters");
            targetSpaceShip.GetMovementSystem.SetMovementParameters(newMovementParameters);
        }

        SpaceShipMovementParameters finalMovementParameters = targetSpaceShip.GetMovementSystem.GetMovementParameters;
        if (finalMovementParameters != null)
        {
            showMovementParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showMovementParameters, (showMovementParameters ? "Close" : "Open") + " Movement Parameters", showMovementParameters ? EditorStyles.boldLabel : null);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (showMovementParameters)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.BeginVertical("box");
                SpaceShipMovementParametersInspector.ShowMovementParameters(finalMovementParameters, 10);
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel -= 1;

                GUILayout.Space(8);
                EditorStaticMethods.DrawLine(2, Color.black);
            }
        }
        #endregion

        #region Shooting
        GUILayout.Space(8);
        EditorGUILayout.LabelField("Shooting System", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        Transform newWeaponsParent = (Transform)EditorGUILayout.ObjectField("Weapons Parent", targetSpaceShip.GetShootingSystem.GetWeaponsParent, typeof(Transform), true);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetSpaceShip, "Undo Affect Weapons Parent");
            targetSpaceShip.GetShootingSystem.SetWeaponsParent(newWeaponsParent);
        }

        EditorGUI.BeginChangeCheck();

        WeaponParameters newWeaponParameters = (WeaponParameters)EditorGUILayout.ObjectField("Weapon Parameters", targetSpaceShip.GetShootingSystem.GetBaseWeaponParameters, typeof(WeaponParameters), false);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetSpaceShip, "Undo Affect Weapons Parameters");
            targetSpaceShip.GetShootingSystem.SetWeaponParameters(newWeaponParameters);
        }

        WeaponParameters finalWeaponParameters = targetSpaceShip.GetShootingSystem.GetBaseWeaponParameters;

        if (finalWeaponParameters != null)
        {
            showWeaponParameters = EditorGUILayout.BeginFoldoutHeaderGroup(showWeaponParameters, (showWeaponParameters ? "Close" : "Open") + " Weapon Parameters", showWeaponParameters ? EditorStyles.boldLabel : null);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (showWeaponParameters)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.BeginVertical("box");
                WeaponParametersInspector.ShowWeaponParameters(finalWeaponParameters, 15, ref showShootParameters, ref showProjectileParameters);
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel -= 1;

                GUILayout.Space(8);
                EditorStaticMethods.DrawLine(2, Color.black);
            }
        }
        #endregion

        #region Others
        GUILayout.Space(8);
        EditorGUILayout.LabelField("Other References", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        DamageableComponent newLinkedDamagesComponent = (DamageableComponent)EditorGUILayout.ObjectField("Linked Damageable Component", targetSpaceShip.GetRelatedDamageableComponent, typeof(DamageableComponent), true);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetSpaceShip, "Change Damageable Component");
            targetSpaceShip.SetRelatedDamageableComponent(newLinkedDamagesComponent);
        }

        if (targetSpaceShip.GetRelatedDamageableComponent != null)
        {
            EditorGUI.BeginChangeCheck();

            int newLifeAmount = EditorGUILayout.IntField("Ship Life Amount", targetSpaceShip.GetRelatedDamageableComponent.GetMaxLifeAmount);

            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Life Amount Changed");
                Undo.RecordObject(targetSpaceShip.GetRelatedDamageableComponent, "Undo Change Max Life Amount");
                targetSpaceShip.GetRelatedDamageableComponent.SetMaxLifeAmount(newLifeAmount);
            }
        }

        #endregion

        if (EditorGUI.EndChangeCheck())
            EditorSceneManager.MarkSceneDirty(targetSpaceShip.gameObject.scene);

        //base.OnInspectorGUI();
    }
}
