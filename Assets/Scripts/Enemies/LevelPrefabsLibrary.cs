using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prefabs Library", menuName = "DO A BARELL ROLL/Scriptables/Prefab Library")]
public class LevelPrefabsLibrary : ScriptableObject
{
    [SerializeField] List<LevelPrefabInformations> obstaclesPrefabInformations = new List<LevelPrefabInformations>();
    public List<LevelPrefabInformations> GetObstaclePrefabInformations { get { return obstaclesPrefabInformations; } }

    [SerializeField] List<LevelPrefabInformations> enemyPrefabInformations = new List<LevelPrefabInformations>();
    public List<LevelPrefabInformations> GetEnemyPrefabInformations { get { return enemyPrefabInformations; } }

    [SerializeField] List<LevelPrefabInformations> projectilePrefabInformations = new List<LevelPrefabInformations>();
    public List<LevelPrefabInformations> GetProjectilePrefabInformations { get { return projectilePrefabInformations; } }

    public void AddEnemyPrefabInformations(GameObject prefab, Color identifyingColor)
    {
        enemyPrefabInformations.Add(new LevelPrefabInformations(prefab, identifyingColor));
    }

    public static void DebugPrefabInformations(LevelPrefabInformations infos)
    {
        Debug.Log("Enemy : " + infos.elementPrefab);
    }
}

[System.Serializable]
public class LevelPrefabInformations
{
    public LevelPrefabInformations(GameObject prefab, Color color)
    {
        elementName = prefab.name;
        elementPrefab = prefab;
        elementAttributedColor = color;
    }

    public string elementName = "Element Name";
    public GameObject elementPrefab = default;
    public Color elementAttributedColor = Color.red;
}