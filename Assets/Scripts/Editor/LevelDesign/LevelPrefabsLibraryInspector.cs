using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelPrefabsLibrary))]
public class LevelPrefabsLibraryInspector : Editor
{
    LevelPrefabsLibrary targetLibrary;

    private void OnEnable()
    {
        targetLibrary = target as LevelPrefabsLibrary;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(16);

        if (GUILayout.Button("Show Library"))
        {
            GenericMenu menu = new GenericMenu();

            List<LevelPrefabInformations> infos = targetLibrary.GetEnemyPrefabInformations;
            for (int i = 0; i < infos.Count; i++)
            {
                menu.AddItem(new GUIContent(infos[i].elementName), false, DebugLibraryElement, i);
            }
            menu.AddItem(new GUIContent(infos[0].elementName), false, DebugLibraryElement, 0);

            /*menu.AddItem(new GUIContent("First Category/First Option"), false, DebugLibraryElement, 0);
            menu.AddItem(new GUIContent("First Category/Second Option"), false, DebugLibraryElement, 1);
            menu.AddItem(new GUIContent("Second Category/First Option"), false, DebugLibraryElement, 2);
            menu.AddItem(new GUIContent("Second Category/Second Option"), false, DebugLibraryElement, 3);*/

            menu.ShowAsContext();
        }
    }

    void DebugLibraryElement(object index)
    {
        LevelPrefabsLibrary.DebugPrefabInformations(targetLibrary.GetEnemyPrefabInformations[(int)index]);
    }
}
