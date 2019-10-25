using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestsCreationWindow : EditorWindow
{
    [MenuItem("Window/DO A BARELL ROLL/Quests Creation Window", priority = 0)]
    public static void OpenWindow()
    {
        OpenWindow(null);
    }

    public static void OpenWindow(string folderPath)
    {
        QuestsCreationWindow window = QuestsCreationWindow.GetWindow(typeof(QuestsCreationWindow)) as QuestsCreationWindow;
        window.Init(folderPath);
    }

    DefaultAsset selectedFolderRef;
    bool createSetFolders = true;
    TextAsset questsSpreadsheet = default;

    public void Init(string folderPath)
    {
        Show();
        selectedFolderRef = AssetDatabase.LoadAssetAtPath(folderPath, typeof(DefaultAsset)) as DefaultAsset;
        createSetFolders = true;
    }

    private void OnGUI()
    {
        selectedFolderRef = EditorGUILayout.ObjectField("Target Folder", selectedFolderRef, typeof(DefaultAsset), false) as DefaultAsset;
        questsSpreadsheet = EditorGUILayout.ObjectField("Used Quests Spreasheet", questsSpreadsheet, typeof(TextAsset), false) as TextAsset;
        createSetFolders = EditorGUILayout.Toggle("Generate All Set Folders", createSetFolders);

        GUILayout.Space(16);

        if (selectedFolderRef != null && questsSpreadsheet != null)
        {
            if (GUILayout.Button("Generate Quests"))
            {
                GenerateQuests();
            }
        }
    }

    public void GenerateQuests()
    {
        string rawContent = questsSpreadsheet.text;

        string[] lineList = rawContent.Split(new string[] { "\n" }, System.StringSplitOptions.None);

        string baseTargetFolderPath = selectedFolderRef.GetFolderPath();
        string finalTargetFolderPath = baseTargetFolderPath;

        Object questsLibraryObject = Resources.Load("Quests/Quests Library");
        QuestsLibrary questsLibrary = questsLibraryObject != null ? questsLibraryObject as QuestsLibrary : null;

        for (int i = 1; i < lineList.Length; i++)
        {
            finalTargetFolderPath = baseTargetFolderPath;

            string rawLine = lineList[i];

            Quest newQuest = ScriptableObject.CreateInstance<Quest>();

            string[] rawElements = rawLine.Split(new string[] { ";" }, System.StringSplitOptions.None);

            if (rawElements.Length < 6)
                break;

            newQuest.questName = rawElements[0];
            newQuest.questDescription = rawElements[1];

            newQuest.questType = (QuestType)System.Enum.Parse(typeof(QuestType), rawElements[2], true);
            newQuest.questRealisationType = (QuestRealisationType)System.Enum.Parse(typeof(QuestRealisationType), rawElements[3], true);

            int valueToReach = 0;
            int.TryParse(rawElements[4], out valueToReach);
            newQuest.valueToReach = valueToReach;

            string setName = rawElements[5];
            if (setName[setName.Length - 1].GetHashCode() == 851981)
                setName = setName.Substring(0, setName.Length - 1);

            if (createSetFolders && !string.IsNullOrEmpty(setName))
            {
                finalTargetFolderPath += "/" + setName;

                if (!AssetDatabase.IsValidFolder(finalTargetFolderPath))
                    AssetDatabase.CreateFolder(baseTargetFolderPath, setName);
            }

            EditorStaticMethods.CreateOrReplaceQuestInFolder(finalTargetFolderPath, newQuest);

            if (questsLibrary != null)
                questsLibrary.AddToLibrary(newQuest, setName);
        }

        Debug.Log("Succesfully loaded !");
    }
}