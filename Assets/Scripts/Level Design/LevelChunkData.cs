using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Chunk", menuName = "DO A BARELL ROLL/Scriptables/Level Design/Level Chunk")]
public class LevelChunkData : ScriptableObject
{
    [SerializeField] LevelPrefabsLibrary prefabsLibrary;
    public LevelPrefabsLibrary GetPrefabLibrary { get { return prefabsLibrary; } }

    [ContextMenu("DebugChunkSize")]
    public void DebugChunkSize()
    {
        Debug.Log("Chunk Size : " + numberOfColumn + "x" + numberOfLines);
        Debug.Log("Content Size : " + chunkContent.Count);
    }

    private void Awake()
    {
        prefabsLibrary = Resources.Load("Level Prefabs Library") as LevelPrefabsLibrary;

        UpdateGridSize();
    }


    [HideInInspector] [SerializeField] int numberOfLines = 10;
    int numberOfColumn = 9;
    public int GetNumberOfColumn { get { return numberOfColumn; } }

    [SerializeField] List<int> chunkContent;
    public List<int> GetChunkContent { get { return chunkContent; } }
    public void SetChunkContent(List<int>  newContent) { chunkContent = newContent; }

    public List<List<int>> ListIntoTab(List<int> list)
    {
        List<List<int>> tab = new List<List<int>>();

        List<int> temp = new List<int>();
        for (int i=0; i<list.Count; i++)
        {
            temp.Add(list[i]);
            if (i % numberOfColumn == (numberOfColumn-1))
            {
                tab.Add(temp);
                temp = new List<int>();
            }
        }

        if (temp.Count != 0)
        {
            while (temp.Count < numberOfColumn)
                temp.Add(0);

            tab.Add(temp);
        }

        return tab;
    }

    public List<int> TabIntoList(List<List<int>> tab)
    {
        List<int> list = new List<int>();

        foreach (List<int> line in tab)
        {
            foreach (int tabValue in line)
                list.Add(tabValue);
        }

        return list;
    }

    public void UpdateGridSize()
    {
        if (chunkContent == null)
            chunkContent = new List<int>();

        List<List<int>> chunkTab = ListIntoTab(chunkContent);
        List<int> baseLine = new List<int>();

        for (int i = 0; i < numberOfColumn; i++)
            baseLine.Add(0);

        while (chunkTab.Count < numberOfLines)
            chunkTab.Add(baseLine);

        while (chunkTab.Count > numberOfLines)
            chunkTab.RemoveAt(chunkTab.Count - 1);

        chunkContent = TabIntoList(chunkTab);


        //DebugChunkSize();
    }
}