using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelChunkCreationWindow : EditorWindow
{
    /*[MenuItem("Window/DO A BARELL ROLL/Level Chunk Creation Window", priority = 0)]
    public static void OpenWindow()
    {
        OpenWindow(null);
    }

    public static void OpenWindow(string folderPath)
    {
        LevelChunkCreationWindow window = LevelChunkCreationWindow.GetWindow(typeof(LevelChunkCreationWindow)) as LevelChunkCreationWindow;
        window.Init(folderPath);
    }

    DefaultAsset selectedFolderRef;
    LevelPrefabsLibrary prefabsLibrary;

    /// <summary>
    /// Will contain the indexes of the chunk inside a tab of ints (each int = one level element)
    /// </summary>
    List<List<int>> chunkContent;

    int numberOfColumn = 9;
    int numberOfLines;
    public void Init(string folderPath)
    {
        Show();
        selectedFolderRef = AssetDatabase.LoadAssetAtPath(folderPath, typeof(DefaultAsset)) as DefaultAsset;

        numberOfColumn = 9;
        numberOfLines = 10;

        chunkContent = new List<List<int>>();
        List<int> baseLine = new List<int>(9);
        for (int i = 0; i < numberOfColumn; i++)
            baseLine.Add(0);

        for (int i = 0; i < numberOfLines; i++)
            chunkContent.Add(baseLine);
    }

    private void OnGUI()
    {
        selectedFolderRef = EditorGUILayout.ObjectField("Target Folder", selectedFolderRef, typeof(DefaultAsset)) as DefaultAsset;

        numberOfLines = EditorGUILayout.IntField("Number of Lines", numberOfLines);//


        Vector2 gridStartPosition = new Vector2();

        ShowChunkGrid(gridStartPosition);
    }

    public void ShowChunkGrid(Vector2 basePosition)
    {
        EditorGUILayout.BeginVertical();

        float spaceBetween = 3;
        float tileHeight = 10;

        EditorGUI.DrawRect(new Rect(50, 50, 100, 100), Color.blue);

        EditorGUILayout.LabelField(Event.current.mousePosition.ToString());
        if (Event.current.type == EventType.KeyDown) Repaint();

        if (chunkContent == null) return;

        for(int y = 0; y < chunkContent.Count; y++)
        {

        }
        EditorGUILayout.EndVertical();
    }*/
}
