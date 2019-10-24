using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SpaceShipBarellRollValues))]
public class SpaceShipBarellRollValuesDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty canBarellRoll = property.FindPropertyRelative("canDoABarellRool");

        return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2) * (canBarellRoll.boolValue ? 4 : 2);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int baseIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        position.xMin += baseIndentLevel * 15;

        float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        float oldLabelWidth = EditorGUIUtility.labelWidth;

        float horizontalSpaceBewteen = 10f;
        float verticalSpaceBewteen = lineHeight + EditorGUIUtility.standardVerticalSpacing;
        float totaldWidth = (position.width - horizontalSpaceBewteen);
        float midWidth = (position.width - horizontalSpaceBewteen) * 0.5f - horizontalSpaceBewteen / 2;

        float canBarellRollWidth = (position.width - horizontalSpaceBewteen) * 0.4f - horizontalSpaceBewteen / 2;
        float cooldownWidth = (position.width - horizontalSpaceBewteen) * 0.6f - horizontalSpaceBewteen / 2;

        Rect labelRect = new Rect(position.x, position.y, position.width, lineHeight);
        Rect canBarellRect = new Rect(position.x, position.y + verticalSpaceBewteen, canBarellRollWidth, lineHeight);

        GUI.Label(labelRect, new GUIContent("Barell Roll Values"), EditorStyles.boldLabel);

        EditorGUIUtility.labelWidth = oldLabelWidth * 0.6f;
        SerializedProperty canBarellRoll = property.FindPropertyRelative("canDoABarellRool");
        EditorGUI.PropertyField(canBarellRect, canBarellRoll, new GUIContent("Can Barell Roll"));

        if (canBarellRoll.boolValue)
        {
            EditorGUIUtility.labelWidth = oldLabelWidth * 0.5f;

            SerializedProperty barellRollCooldown = property.FindPropertyRelative("barellRollCooldown");
            SerializedProperty barellRollDistance = property.FindPropertyRelative("barellRollDistance");
            SerializedProperty barellRollDuration = property.FindPropertyRelative("barellRollDuration");
            SerializedProperty barellRollCurve = property.FindPropertyRelative("barellRollCurve");
            
            Rect cooldownRect = new Rect(position.x + horizontalSpaceBewteen + canBarellRect.width, position.y + verticalSpaceBewteen, cooldownWidth, lineHeight);
            EditorGUI.PropertyField(cooldownRect, barellRollCooldown, new GUIContent("Cooldown"));

            Rect distanceRect = new Rect(position.x, position.y + verticalSpaceBewteen * 2, midWidth, lineHeight);
            EditorGUI.PropertyField(distanceRect, barellRollDistance, new GUIContent("Distance"));
            Rect durationRect = new Rect(position.x + horizontalSpaceBewteen + distanceRect.width, position.y + verticalSpaceBewteen * 2, midWidth, lineHeight);
            EditorGUI.PropertyField(durationRect, barellRollDuration, new GUIContent("Duration"));

            EditorGUIUtility.labelWidth = oldLabelWidth;


            Rect curveRect = new Rect(position.x, position.y + verticalSpaceBewteen * 3, totaldWidth, lineHeight);
            EditorGUI.PropertyField(curveRect, barellRollCurve, new GUIContent("Movement Curve"));
        }

        EditorGUI.indentLevel = baseIndentLevel;
    }
}