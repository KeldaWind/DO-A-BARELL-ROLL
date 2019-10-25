using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxFloat))]
public class MinMaxFloatDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + 2;
        //return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        SerializedProperty mode = property.FindPropertyRelative("mode");
        MinMaxMode minMaxMode = (MinMaxMode)mode.enumValueIndex;

        float spaceBewteen = 10f;
        float labelWidth= (position.width - spaceBewteen) * 0.35f;
        float minMaxWidth= (position.width - spaceBewteen) * (minMaxMode == MinMaxMode.RandomBetweenMinAndMax ? 0.25f : 0.5f);
        float modeWidth= (position.width - spaceBewteen) * 0.1f;

        Rect labelRect= new Rect(position.x, position.y, labelWidth, lineHeight);
        Rect minRect = new Rect(position.x + spaceBewteen + labelRect.width, position.y, minMaxWidth, lineHeight);
        Rect maxRect= new Rect(position.x + spaceBewteen * 2 + labelRect.width + minRect.width, position.y, minMaxWidth, lineHeight);
        Rect modeRect= new Rect(position.x + labelRect.width + minRect.width + (minMaxMode == MinMaxMode.RandomBetweenMinAndMax ? maxRect.width + spaceBewteen * 3 : spaceBewteen * 2), position.y, modeWidth, lineHeight);
        
        GUI.Label(labelRect, label);

        SerializedProperty min = property.FindPropertyRelative("minValue");
        EditorGUI.PropertyField(minRect, min, GUIContent.none);

        if (minMaxMode == MinMaxMode.RandomBetweenMinAndMax)
        {
            SerializedProperty max = property.FindPropertyRelative("maxValue");
            EditorGUI.PropertyField(maxRect, max, GUIContent.none);
        }

        EditorGUI.PropertyField(modeRect, mode, GUIContent.none);
    }
}
