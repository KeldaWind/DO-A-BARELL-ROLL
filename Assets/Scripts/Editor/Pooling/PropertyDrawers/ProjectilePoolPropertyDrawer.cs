using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ProjectilePool))]
public class ProjectilePoolPropertyDrawer : PoolPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float oldWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth /= 2;

        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        float spaceBewteen = 10f;
        float fieldWidth = (position.width - spaceBewteen) * 0.5f;

        Rect sliderRect = new Rect(position.x, position.y, fieldWidth, EditorGUIUtility.singleLineHeight);
        SerializedProperty numberOfElementsProperty = property.FindPropertyRelative("projectilePrefab");
        EditorGUI.PropertyField(sliderRect, numberOfElementsProperty);

        EditorGUIUtility.labelWidth = oldWidth;

        base.OnGUI(position, property, label);
    }
}
