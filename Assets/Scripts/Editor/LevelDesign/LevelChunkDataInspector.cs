using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelChunkData))]
public class LevelChunkDataInspector : Editor
{
    bool showBrutGrid;

    LevelChunkData targetChunkData;
    SerializedProperty numberOfLinesProperty;
    float numberOfColumns;
    bool leftMouseIsDown;
    bool rightMouseIsDown;
    Vector2 currentTargetTileIndexes = new Vector2(-1, -1);

    LevelChunkBrushType currentBrushType;
    int currentBrushIndex;

    LevelPrefabsLibrary prefabLibrary;
    Dictionary<int, LevelPrefabInformations> prefabsDictionnary;

    private void OnEnable()
    {
        targetChunkData = target as LevelChunkData;
        numberOfLinesProperty = serializedObject.FindProperty("numberOfLines");
        numberOfColumns = targetChunkData.GetNumberOfColumn;
        prefabLibrary = targetChunkData.GetPrefabLibrary;
        ComposePrefabsDictionary();

        if (prefabsDictionnary.ContainsKey(100))
        {
            currentBrushType = LevelChunkBrushType.Obstacle;
            SetCurrentBrushIndex(100);
        }
        else if(prefabsDictionnary.ContainsKey(200))
        {
            currentBrushType = LevelChunkBrushType.Enemy;
            SetCurrentBrushIndex(200);
        }
    }

    public void ComposePrefabsDictionary()
    {
        prefabsDictionnary = new Dictionary<int, LevelPrefabInformations>();
        List<LevelPrefabInformations> obstaclesInfos = prefabLibrary.GetObstaclePrefabInformations;

        for (int i = 0; i < obstaclesInfos.Count; i++)
        {
            LevelPrefabInformations obstacleInfo = obstaclesInfos[i];
            int index = 100 + i;

            if (obstacleInfo.elementPrefab == null)
                continue;

            prefabsDictionnary.Add(index, obstacleInfo);
        }

        List<LevelPrefabInformations> enemiesInfos = prefabLibrary.GetEnemyPrefabInformations;
        for (int i = 0; i < enemiesInfos.Count; i++)
        {
            LevelPrefabInformations enemyInfo = enemiesInfos[i];
            int index = 200 + i;

            if (enemyInfo.elementPrefab == null)
                continue;

            prefabsDictionnary.Add(index, enemyInfo);
        }
    }

    public void GenerateBrushChoiceMenu()
    {
        GenericMenu menu = new GenericMenu();

        switch (currentBrushType)
        {
            case (LevelChunkBrushType.Obstacle):
                foreach (int key in prefabsDictionnary.Keys)
                {
                    if (key >= 100 && key < 200)
                        menu.AddItem(new GUIContent(prefabsDictionnary[key].elementName), false, SetCurrentBrushIndex, key);
                }
                break;

            case (LevelChunkBrushType.Enemy):
                foreach (int key in prefabsDictionnary.Keys)
                {
                    if (key >= 200 && key < 300)
                        menu.AddItem(new GUIContent(prefabsDictionnary[key].elementName), false, SetCurrentBrushIndex, key);
                }
                break;
        }

        menu.ShowAsContext();
    }

    public void SetCurrentBrushIndex(object newBrushIndex)
    {
        currentBrushIndex = (int)newBrushIndex;

        if (!prefabsDictionnary.ContainsKey(currentBrushIndex))
        {
            currentBrushIndex = 0;
            return;
        }

        LevelPrefabInformations linkedInfos = prefabsDictionnary[currentBrushIndex];

        if (currentBrushIndex >= 100 && currentBrushIndex < 200)
        {
            currentBrushType = LevelChunkBrushType.Obstacle;
        }
        else if (currentBrushIndex >= 200 && currentBrushIndex < 300)
        {
            currentBrushType = LevelChunkBrushType.Enemy;
        }
    }

    public override void OnInspectorGUI()
    {
        showBrutGrid = EditorGUILayout.Toggle("Show Brut Grid", showBrutGrid);
        Color baseGUIColor = GUI.color;

        serializedObject.Update();

        bool gridSizeChanged = false;
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(numberOfLinesProperty);
        if (numberOfLinesProperty.intValue < 1)
            numberOfLinesProperty.intValue = 1;

        if (EditorGUI.EndChangeCheck())
            gridSizeChanged = true;

        if (showBrutGrid)
            ShowBrutChunkGrid();
        else
        {
            GUILayout.Space(12);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            GUILayout.Label("GRID PAINTING", style);
            GUILayout.Space(8);

            EditorGUI.BeginChangeCheck();
            currentBrushType = (LevelChunkBrushType)EditorGUILayout.EnumPopup("Brush Type", currentBrushType);
            if (EditorGUI.EndChangeCheck())
            {
                switch (currentBrushType)
                {
                    case (LevelChunkBrushType.Obstacle):
                        if (prefabsDictionnary.ContainsKey(100))
                            currentBrushIndex = 100;
                        else
                            currentBrushIndex = 0;
                        break;

                    case (LevelChunkBrushType.Enemy):
                        if (prefabsDictionnary.ContainsKey(200))
                            currentBrushIndex = 200;
                        else
                            currentBrushIndex = 0;
                        break;
                }
            }


            if (GUILayout.Button("Choose Brush"))
                GenerateBrushChoiceMenu();

            LevelPrefabInformations selectedPrefab = null;

            if (prefabsDictionnary.ContainsKey(currentBrushIndex))
                selectedPrefab = prefabsDictionnary[currentBrushIndex];
            else
                currentBrushIndex = 0;


            EditorGUILayout.BeginHorizontal();
            GUIStyle selectedBrushLabelStyle = new GUIStyle();
            selectedBrushLabelStyle.normal.textColor = selectedPrefab != null ? selectedPrefab.elementAttributedColor : Color.grey;
            selectedBrushLabelStyle.fontStyle = FontStyle.Bold;
            selectedBrushLabelStyle.fontSize = 16;
            selectedBrushLabelStyle.alignment = TextAnchor.LowerCenter;

            EditorGUILayout.BeginVertical();
            GUILayout.Space(12);
            GUILayout.Label(selectedPrefab != null ? "Selected Brush : " + selectedPrefab.elementName : "No brush selected", selectedBrushLabelStyle);
            EditorGUILayout.EndVertical();


            GUILayout.Space(8);
            EditorGUILayout.HelpBox("Alt + Left Clic to copy a Brush", MessageType.Info);
            EditorGUILayout.EndHorizontal();

            Vector2 gridStartPos = new Vector2(10, 180);
            ShowChunkGrid(gridStartPos);
        }

        serializedObject.ApplyModifiedProperties();

        if (gridSizeChanged)
            targetChunkData.UpdateGridSize();
    }

    public void ShowChunkGrid(Vector2 gridStartPos)
    {
        Color baseGUIColor = GUI.color;

        EditorGUI.BeginChangeCheck();
        List<List<int>> chunkTab = targetChunkData.ListIntoTab(targetChunkData.GetChunkContent);

        float spacing = 5;
        float tileWidth = (EditorGUIUtility.currentViewWidth / numberOfColumns)- 10;
        float tileHeight = tileWidth;

        GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, (chunkTab.Count + 1) * (tileHeight + spacing) + 20);

        bool needToRepaint = false;
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            leftMouseIsDown = true;
        else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            leftMouseIsDown = false;

        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
                rightMouseIsDown = true;
        else if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
                rightMouseIsDown = false;

        for (int y = 0; y < chunkTab.Count; y++)
        {
            List<int> line = chunkTab[y];
            for (int x = 0; x < line.Count; x++) 
            {
                int oldValue = chunkTab[y][x];

                Rect rect = new Rect(gridStartPos.x + (tileWidth + spacing) * x, gridStartPos.y + (tileWidth + spacing) * y, tileWidth, tileHeight);

                bool leftMouseIsDownOnRect = false;
                bool rightMouseIsDownOnRect = false;


                if (leftMouseIsDown)
                {
                    leftMouseIsDownOnRect = EditorStaticMethods.CheckIfMouseInRect(rect);
                    if (!Event.current.alt)
                    {
                        if (currentTargetTileIndexes != new Vector2(x, y))
                            currentTargetTileIndexes = new Vector2(x, y);

                        if (leftMouseIsDownOnRect)
                            chunkTab[y][x] = currentBrushIndex;
                    }
                    else
                    {
                        if (leftMouseIsDownOnRect)
                        {
                            SetCurrentBrushIndex(chunkTab[y][x]);
                            needToRepaint = true;
                        }
                    }
                }
                else if (rightMouseIsDown)
                {
                    rightMouseIsDownOnRect = EditorStaticMethods.CheckIfMouseInRect(rect);
                    if (currentTargetTileIndexes != new Vector2(x, y))
                        currentTargetTileIndexes = new Vector2(x, y);

                    if (rightMouseIsDownOnRect)
                        chunkTab[y][x] = 0;
                }

                int tileValue = chunkTab[y][x];
                
                GUIStyle labelStyle = new GUIStyle();
                labelStyle.alignment = TextAnchor.MiddleCenter;
                labelStyle.fontSize = (int)(tileWidth * 0.2f);

                Color tileColor = Color.white;
                string tileName = "/";

                if (prefabsDictionnary.ContainsKey(tileValue))
                {
                    LevelPrefabInformations tileInfo = prefabsDictionnary[tileValue];

                    tileColor = tileInfo.elementAttributedColor;
                    tileName = tileInfo.elementName;
                }

                EditorGUI.DrawRect(rect, tileColor);
                EditorGUI.LabelField(rect, tileName, labelStyle);

                if (oldValue != tileValue)
                    needToRepaint = true;
            }
        }

        if (needToRepaint)
        {
            Undo.RecordObject(targetChunkData, "Undo Chunk Modification");
            targetChunkData.SetChunkContent(targetChunkData.TabIntoList(chunkTab));
            EditorUtility.SetDirty(targetChunkData);
            Repaint();
        }

        GUI.color = baseGUIColor;
    }

    public void ShowBrutChunkGrid()
    {
        EditorGUI.BeginChangeCheck();
        List<List<int>> chunkTab = targetChunkData.ListIntoTab(targetChunkData.GetChunkContent);
        for (int y = 0; y < chunkTab.Count; y++)
        {
            EditorGUILayout.BeginHorizontal();
            List<int> line = chunkTab[y];
            for (int x = 0; x < line.Count; x++)
            {
                chunkTab[y][x] = EditorGUILayout.IntField(chunkTab[y][x]);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (EditorGUI.EndChangeCheck())
        {
            targetChunkData.SetChunkContent(targetChunkData.TabIntoList(chunkTab));
            EditorUtility.SetDirty(targetChunkData);
        }
    }
}

public enum LevelChunkBrushType { Obstacle, Enemy }