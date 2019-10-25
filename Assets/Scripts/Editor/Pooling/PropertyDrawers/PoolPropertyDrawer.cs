using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Pool))]
public class PoolPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 1.5f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        float spaceBewteen = 10f;
        float sliderWidth = (position.width - spaceBewteen) * 0.5f;

        Rect sliderRect = new Rect(position.x + (position.width - spaceBewteen) * 0.5f, position.y, sliderWidth, EditorGUIUtility.singleLineHeight);
        SerializedProperty numberOfElementsProperty = property.FindPropertyRelative("numberOfElements");
        EditorGUI.PropertyField(sliderRect, numberOfElementsProperty, GUIContent.none);
    }
}
