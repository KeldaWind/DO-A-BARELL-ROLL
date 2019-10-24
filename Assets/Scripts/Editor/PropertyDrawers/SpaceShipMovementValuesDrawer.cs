using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SpaceShipBaseMovementValues))]
public class SpaceShipBaseMovementValuesDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2) * 3;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int baseIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        position.xMin += baseIndentLevel * 15;

        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        float horizontalSpaceBewteen = 10f;
        float verticalSpaceBewteen = lineHeight + EditorGUIUtility.standardVerticalSpacing;
        float maxSpeedWidth = (position.width - horizontalSpaceBewteen);
        float accelerationWidth = (position.width - horizontalSpaceBewteen) * 0.5f - horizontalSpaceBewteen / 2;

        Rect labelRect = new Rect(position.x, position.y, position.width, lineHeight);
        Rect maxSpeedRect = new Rect(position.x, position.y + verticalSpaceBewteen, maxSpeedWidth, lineHeight);
        Rect accelerationRect = new Rect(position.x, position.y + verticalSpaceBewteen * 2, accelerationWidth, lineHeight);
        Rect descelerationRect = new Rect(position.x + horizontalSpaceBewteen + accelerationRect.width, position.y + verticalSpaceBewteen * 2, accelerationWidth, lineHeight);

        GUI.Label(labelRect, new GUIContent("Base Spaceship Movement Values"), EditorStyles.boldLabel);
        
        SerializedProperty maxSpeed = property.FindPropertyRelative("maxSpeed");
        EditorGUI.PropertyField(maxSpeedRect, maxSpeed, new GUIContent("Maximum Speed"));

        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth /= 2;

        SerializedProperty acceleration = property.FindPropertyRelative("acceleration");
        EditorGUI.PropertyField(accelerationRect, acceleration, new GUIContent("Acceleration"));

        SerializedProperty desceleration = property.FindPropertyRelative("desceleration");
        EditorGUI.PropertyField(descelerationRect, desceleration, new GUIContent("Desceleration"));

        EditorGUIUtility.labelWidth = oldLabelWidth;

        EditorGUI.indentLevel = baseIndentLevel;
    }
}