using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), speedAttribute, 
            new GUIContent(projParameters.GetProjectileSpeed.mode == MinMaxMode.Constant ? "Projectile Speed" : "Random Projectile Speed",
            projParameters.GetProjectileSpeed.mode == MinMaxMode.Constant ? "The speed of the projectile" : "The speed of the projectile, randomly picked between the two Values"));

        if (projParameters.GetProjectileSpeed.minValue < 0)
            projParameters.ChangeProjectileMinSpeed(0);

        if (projParameters.GetProjectileSpeed.maxValue < 0)
            projParameters.ChangeProjectileMaxSpeed(0);

        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), lifetimeAttribute,
            new GUIContent(projParameters.GetProjectileLifetime.mode == MinMaxMode.Constant ? "Projectile Lifetime" : "Random Projectile Lifetime",
            projParameters.GetProjectileLifetime.mode == MinMaxMode.Constant ? "The lifeTime of the projectile" : "The lifeTime of the projectile, randomly picked between the two Values"));
        
        if (projParameters.GetProjectileLifetime.minValue < 0.01f)
            projParameters.ChangeProjectileMinLifetime(0.01f);

        if (projParameters.GetProjectileLifetime.maxValue < 0.01f)
            projParameters.ChangeProjectileMaxLifetime(0.01f);

        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), sizeAttribute);
        if (projParameters.GetProjectileSize < 0.01f)
            projParameters.ChangeSize(0.01f);

        EditorGUI.PropertyField(EditorStaticMethods.GetIndentedControlRect(indentValue), damagesAttribute);
        if (projParameters.GetProjectileDamages< 1)
            projParameters.ChangeDamages(1);

        serializedParameters.ApplyModifiedProperties();
    }
}
