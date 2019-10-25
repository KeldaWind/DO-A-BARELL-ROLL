using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LevelPrefabInformations))]
public class LevelPrefabInformationsPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 1.5f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float spaceBewteen = 10f;
        float thirdWidth = (position.width - spaceBewteen) * 0.3f;

        SerializedProperty nameProperty = property.FindPropertyRelative("elementName");
        SerializedProperty prefabProperty = property.FindPropertyRelative("elementPrefab");
        SerializedProperty colorProperty = property.FindPropertyRelative("elementAttributedColor");

        Color baseColor = GUI.color;
        GUI.color = Color.Lerp(baseColor, colorProperty.colorValue, 0.5f);

        Rect nameRect = new Rect(position.x, position.y, thirdWidth, EditorGUIUtility.singleLineHeight);
        Rect prefabRect = new Rect(position.x + thirdWidth + spaceBewteen, position.y, thirdWidth, EditorGUIUtility.singleLineHeight);
        Rect colorRect = new Rect(position.x + (thirdWidth + spaceBewteen) * 2, position.y, thirdWidth, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(nameRect, nameProperty, GUIContent.none);
        EditorGUI.PropertyField(prefabRect, prefabProperty, GUIContent.none);
        EditorGUI.PropertyField(colorRect, colorProperty, GUIContent.none);
        GUI.color = baseColor;
    }
}
