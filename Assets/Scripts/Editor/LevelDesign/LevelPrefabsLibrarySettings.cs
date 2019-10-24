using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelPrefabsLibrarySettings : SettingsProvider
{
    static Editor editor;

    public LevelPrefabsLibrarySettings(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope)
    {

    }

    [SettingsProvider]
    static SettingsProvider CreateProvider()
    {
        LevelPrefabsLibrarySettings myProvider = new LevelPrefabsLibrarySettings("Project/Level Prefabs Library", SettingsScope.Project);
        myProvider.guiHandler = LibraryGUI;
        return myProvider;
    }

    static void LibraryGUI(string context)
    {
        ScriptableObject library = Resources.Load("Level Prefabs Library") as ScriptableObject;

        if (!editor)
            Editor.CreateCachedEditor(library, null, ref editor);

        editor.OnInspectorGUI();
    }
}
