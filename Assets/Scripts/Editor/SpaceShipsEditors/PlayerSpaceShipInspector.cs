using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PlayerSpaceShipScript))]
public class PlayerSpaceShipInspector : SpaceShipInspector
{
    PlayerSpaceShipScript targetPlayerSpaceShip;

    public override void OnEnable()
    {
        base.OnEnable();
        targetPlayerSpaceShip = target as PlayerSpaceShipScript;
    }

    bool showInputs;
    Editor currentInputEditor;
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        GUILayout.Space(8);

        serializedObject.Update();
        SerializedProperty damageTagProperty = serializedObject.FindProperty("damageTag");
        EditorGUILayout.PropertyField(damageTagProperty);

        serializedObject.ApplyModifiedProperties();

        GUILayout.Space(8);
        EditorGUILayout.LabelField("Inputs", EditorStyles.boldLabel);
        PlayerShipInputs baseInputs = targetPlayerSpaceShip.GetPlayerShipInputs;
        PlayerShipInputs newInputs =
            (PlayerShipInputs)EditorGUILayout.ObjectField
            ("Inputs", baseInputs, typeof(PlayerShipInputs), false);

        if (newInputs != baseInputs)
        {
            Undo.RecordObject(targetPlayerSpaceShip, "Undo Affect Inputs");
            targetPlayerSpaceShip.SetPlayerShipInputs(newInputs);
            currentInputEditor = null;
        }

        PlayerShipInputs finalInputs = targetPlayerSpaceShip.GetPlayerShipInputs;
        if (finalInputs != null)
        {
            if (currentInputEditor == null)
                Editor.CreateCachedEditor(finalInputs, typeof(PlayerShipInputsInspector), ref currentInputEditor);

            showInputs = EditorGUILayout.BeginFoldoutHeaderGroup(showInputs, (showInputs ? "Close" : "Open") + " Inputs", showInputs ? EditorStyles.boldLabel : null);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (showInputs)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.BeginVertical("box");
                currentInputEditor.OnInspectorGUI();
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel -= 1;
                GUILayout.Space(8);
                EditorStaticMethods.DrawLine(2, Color.black);
            }
        }

        if (EditorGUI.EndChangeCheck())
            EditorSceneManager.MarkSceneDirty(targetPlayerSpaceShip.gameObject.scene);

        base.OnInspectorGUI();
    }
}
