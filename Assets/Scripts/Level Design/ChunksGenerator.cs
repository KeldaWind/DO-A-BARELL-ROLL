using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksGenerator : MonoBehaviour
{
    PoolingManager poolingManager;

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

    private void Start()
    {
        poolingManager = GameManager.gameManager.GetPoolingManager;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            GenerateChunkAtPos(poolingManager, chunks[Random.Range(0, chunks.Length)], testPosition.position, null);
        }*/
    }

    public void CreateNewChunk(Vector3 pivotPosition, Transform chunkEndMark)
    {
        LevelChunkData chunk = chunks[Random.Range(0, chunks.Length)];
        GenerateChunkAtPos(poolingManager, chunk, pivotPosition, chunkEndMark);
    }

    public static void GenerateChunkAtPos(PoolingManager poolingManager, LevelChunkData chunk, Vector3 pivotPosition, Transform chunkEndMark)
    {
        Vector3 currentInstantiationPos = pivotPosition;

        List<List<int>> chunkTab = chunk.ListIntoTab(chunk.GetChunkContent);

        int numberOfLines = chunkTab.Count;
        int numberOfColumns = numberOfLines > 0 ? chunkTab[0].Count : 9;

        if(chunkEndMark != null)
        {
            chunkEndMark.position = currentInstantiationPos + Vector3.up * chunkTab.Count;
        }

        for (int y = 0; y < chunkTab.Count; y++)
        {
            List<int> line = chunkTab[y];
            for (int x = 0; x < line.Count; x++)
            {
                int prefabIndex = line[x];
                if (prefabIndex == 0)
                    continue;

                currentInstantiationPos.x = pivotPosition.x - numberOfColumns/2 + x;
                currentInstantiationPos.y = pivotPosition.y + numberOfLines - y + 0.5f;

                if(prefabIndex >= 100 && prefabIndex < 200)
                {
                    Obstacle newObstacle = poolingManager.GetObstacleFromPool(prefabIndex);
                    if(newObstacle != null)
                    {
                        newObstacle.transform.position = currentInstantiationPos;
                    }
                }
                else if (prefabIndex >= 200 && prefabIndex < 300)
                {
                    EnemySpaceShipScript newEnemy = poolingManager.GetEnemyFromPool(prefabIndex);
                    if (newEnemy != null)
                    {
                        newEnemy.transform.position = currentInstantiationPos;
                    }
                }
            }
        }
    }
}