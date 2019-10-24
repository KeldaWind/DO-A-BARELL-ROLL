using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksGenerator : MonoBehaviour
{
    LevelPrefabsLibrary prefabsLibrary;
    Dictionary<int, LevelPrefabInformations> prefabsDictionnary;
    public void ComposePrefabsDictionary()
    {
        prefabsDictionnary = new Dictionary<int, LevelPrefabInformations>();
        List<LevelPrefabInformations> obstaclesInfos = prefabsLibrary.GetObstaclePrefabInformations;

        for (int i = 0; i < obstaclesInfos.Count; i++)
        {
            LevelPrefabInformations obstacleInfo = obstaclesInfos[i];
            int index = 100 + i;

            if (obstacleInfo.elementPrefab == null)
                continue;

            prefabsDictionnary.Add(index, obstacleInfo);
        }

        List<LevelPrefabInformations> enemiesInfos = prefabsLibrary.GetEnemyPrefabInformations;
        for (int i = 0; i < enemiesInfos.Count; i++)
        {
            LevelPrefabInformations enemyInfo = enemiesInfos[i];
            int index = 200 + i;

            if (enemyInfo.elementPrefab == null)
                continue;

            prefabsDictionnary.Add(index, enemyInfo);
        }
    }

    [SerializeField] Transform testPosition = default;
    [SerializeField] LevelChunkData[] chunks = default;

    private void Awake()
    {
        prefabsLibrary = Resources.Load("Level Prefabs Library") as LevelPrefabsLibrary;
        ComposePrefabsDictionary();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GenerateChunkAtPos(prefabsDictionnary, chunks[Random.Range(0, chunks.Length)], testPosition.position);
        }
    }

    public static void GenerateChunkAtPos(Dictionary<int, LevelPrefabInformations> prefabsDictionnary, LevelChunkData chunk, Vector3 pivotPosition)
    {
        Vector3 currentInstantiationPos = pivotPosition;
        LevelPrefabInformations currentPrefabInfos = null;

        List<List<int>> chunkTab = chunk.ListIntoTab(chunk.GetChunkContent);

        int numberOfLines = chunkTab.Count;
        int numberOfColumns = numberOfLines > 0 ? chunkTab[0].Count : 9;

        for (int y = 0; y < chunkTab.Count; y++)
        {
            List<int> line = chunkTab[y];
            for (int x = 0; x < chunkTab.Count; x++)
            {
                int prefabIndex = line[x];
                if (prefabIndex == 0)
                    continue;

                if (!prefabsDictionnary.ContainsKey(prefabIndex))
                    continue;

                currentInstantiationPos.x = pivotPosition.x - numberOfColumns/2 + x;
                currentInstantiationPos.y = pivotPosition.y + numberOfLines - y;

                currentPrefabInfos = prefabsDictionnary[prefabIndex];

                Instantiate(currentPrefabInfos.elementPrefab, currentInstantiationPos, Quaternion.identity);

                currentPrefabInfos = null;
            }
        }
    }
}