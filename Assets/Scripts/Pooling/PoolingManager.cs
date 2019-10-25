using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [SerializeField] Transform poolsParent = default;
    [SerializeField] List<EnemyPool> enemiesPools = new List<EnemyPool>();
    [SerializeField] List<ObstaclePool> obstaclesPools = new List<ObstaclePool>();
    [SerializeField] List<ProjectilePool> projectilesPools = new List<ProjectilePool>();

    Dictionary<int, Queue<EnemySpaceShipScript>> enemiesPoolsDictionnary;
    Dictionary<int, Queue<Obstacle>> obstaclePoolsDictionnary;
    Dictionary<int, Queue<ProjectileScript>> projectilesPoolsDictionnary;

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach (EnemyPool pool in enemiesPools)
        {
            pool.DestroyPoolParent();
            pool.DestroyPoolObjects();
        }

        foreach (ObstaclePool pool in obstaclesPools)
        {
            pool.DestroyPoolParent();
            pool.DestroyPoolObjects();
        }

        foreach (ProjectilePool pool in projectilesPools)
        {
            pool.DestroyPoolParent();
            pool.DestroyPoolObjects();
        }

        enemiesPools = new List<EnemyPool>();
        obstaclesPools = new List<ObstaclePool>();
        projectilesPools = new List<ProjectilePool>();
    }

    [ContextMenu("AssignPoolValuesWithLibrary")]
    public void AssignPoolValuesWithLibrary()
    {
        foreach (EnemyPool pool in enemiesPools)
        {
            pool.DestroyPoolParent();
            pool.DestroyPoolObjects();
        }

        foreach (ObstaclePool pool in obstaclesPools)
        {
            pool.DestroyPoolParent();
            pool.DestroyPoolObjects();
        }

        foreach (ProjectilePool pool in projectilesPools)
        {
            pool.DestroyPoolParent();
            pool.DestroyPoolObjects();
        }

        enemiesPools = new List<EnemyPool>();
        obstaclesPools = new List<ObstaclePool>();
        projectilesPools = new List<ProjectilePool>();

        LevelPrefabsLibrary levelPrefabsLibrary = Resources.Load("Level Prefabs Library") as LevelPrefabsLibrary;

        if(levelPrefabsLibrary == null)
        {
            Debug.LogError("Library Not Found");
            return;
        }

        List<LevelPrefabInformations> enemiesInformations = levelPrefabsLibrary.GetEnemyPrefabInformations;

        for (int i = 0; i < enemiesInformations.Count; i++)
        {
            LevelPrefabInformations info = enemiesInformations[i];

            if (info.elementPrefab == null)
                continue;

            EnemyPool newEnemyPool = new EnemyPool();

            int index = 200 + i;
            newEnemyPool.elementLibraryIndex = index;
            newEnemyPool.enemyPrefab = info.elementPrefab.GetComponent<EnemySpaceShipScript>();
            newEnemyPool.instantiatedObjects = new List<EnemySpaceShipScript>();
            newEnemyPool.numberOfElements = 20;

            GameObject parent = new GameObject();
            parent.name = info.elementName + " Pool";
            parent.transform.parent = poolsParent;
            newEnemyPool.poolParent = parent.transform;

            enemiesPools.Add(newEnemyPool);
        }

        List<LevelPrefabInformations> obstaclesInformations = levelPrefabsLibrary.GetObstaclePrefabInformations;
        for (int i = 0; i < obstaclesInformations.Count; i++)
        {
            LevelPrefabInformations info = obstaclesInformations[i];

            if (info.elementPrefab == null)
                continue;

            ObstaclePool newObstaclePool = new ObstaclePool();

            int index = 100 + i;
            newObstaclePool.elementLibraryIndex = index;
            newObstaclePool.obstaclePrefab = info.elementPrefab.GetComponent<Obstacle>();
            newObstaclePool.instantiatedObjects = new List<Obstacle>();
            newObstaclePool.numberOfElements = 20;

            GameObject parent = new GameObject();
            parent.name = info.elementName + " Pool";
            parent.transform.parent = poolsParent;
            newObstaclePool.poolParent = parent.transform;

            obstaclesPools.Add(newObstaclePool);
        }

        List<LevelPrefabInformations> projectileInformations = levelPrefabsLibrary.GetProjectilePrefabInformations;
        for (int i = 0; i < projectileInformations.Count; i++)
        {
            LevelPrefabInformations info = projectileInformations[i];

            if (info.elementPrefab == null)
                continue;

            ProjectilePool newProjectilePool = new ProjectilePool();

            int index = 300 + i;
            newProjectilePool.elementLibraryIndex = index;
            newProjectilePool.projectilePrefab = info.elementPrefab.GetComponent<ProjectileScript>();

            newProjectilePool.projectilePrefab.SetPoolingIndex(index);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(newProjectilePool.projectilePrefab);
#endif

            newProjectilePool.instantiatedObjects = new List<ProjectileScript>();
            newProjectilePool.numberOfElements = 20;

            GameObject parent = new GameObject();
            parent.name = info.elementName + " Pool";
            parent.transform.parent = poolsParent;
            newProjectilePool.poolParent = parent.transform;

            projectilesPools.Add(newProjectilePool);
        }

#if UNITY_EDITOR
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }

    [ContextMenu("GeneratePoolsObjects")]
    public void GeneratePoolsObjects()
    {
        foreach (EnemyPool enemyPool in enemiesPools)
        {
            enemyPool.DestroyPoolObjects();

            for(int i = 0; i < enemyPool.numberOfElements; i++)
            {
                EnemySpaceShipScript newEnemy = Instantiate(enemyPool.enemyPrefab, enemyPool.poolParent);
                newEnemy.SetUpOnPoolInstantiation(enemyPool.elementLibraryIndex);
                enemyPool.instantiatedObjects.Add(newEnemy);
            }
        }

        foreach (ObstaclePool obstaclePool in obstaclesPools)
        {
            obstaclePool.DestroyPoolObjects();

            for (int i = 0; i < obstaclePool.numberOfElements; i++)
            {
                Obstacle newObstacle = Instantiate(obstaclePool.obstaclePrefab, obstaclePool.poolParent);
                newObstacle.SetUpOnPoolInstantiation(obstaclePool.elementLibraryIndex);
                obstaclePool.instantiatedObjects.Add(newObstacle);
            }
        }

        foreach (ProjectilePool projectilePool in projectilesPools)
        {
            projectilePool.DestroyPoolObjects();

            for (int i = 0; i < projectilePool.numberOfElements; i++)
            {
                ProjectileScript newProjectile = Instantiate(projectilePool.projectilePrefab, projectilePool.poolParent);
                newProjectile.SetUpOnPoolInstantiation(projectilePool.elementLibraryIndex);
                projectilePool.instantiatedObjects.Add(newProjectile);
            }
        }

#if UNITY_EDITOR
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }

    public void CreatePoolsQueuesAndDictionary()
    {
        enemiesPoolsDictionnary = new Dictionary<int, Queue<EnemySpaceShipScript>>();

        foreach(EnemyPool enemyPool in enemiesPools)
        {
            Queue<EnemySpaceShipScript> newEnemiesQueue = new Queue<EnemySpaceShipScript>();
            foreach (EnemySpaceShipScript enemy in enemyPool.instantiatedObjects)
            {
                enemy.OnReturnToPool = ReturnEnemyInPool;
                enemy.FirstSetUp();
                newEnemiesQueue.Enqueue(enemy);
            }
            enemiesPoolsDictionnary.Add(enemyPool.elementLibraryIndex, newEnemiesQueue);
        }

        obstaclePoolsDictionnary = new Dictionary<int, Queue<Obstacle>>();
        
        foreach (ObstaclePool obstaclePool in obstaclesPools)
        {
            Queue<Obstacle> newObstacleQueue = new Queue<Obstacle>();
            foreach (Obstacle obstacle in obstaclePool.instantiatedObjects)
            {
                obstacle.OnReturnToPool = ReturnObstacleInPool;
                newObstacleQueue.Enqueue(obstacle);
            }
            obstaclePoolsDictionnary.Add(obstaclePool.elementLibraryIndex, newObstacleQueue);
        }

        projectilesPoolsDictionnary = new Dictionary<int, Queue<ProjectileScript>>();

        foreach (ProjectilePool projectilePool in projectilesPools)
        {
            Queue<ProjectileScript> newProjectilesQueue = new Queue<ProjectileScript>();
            foreach (ProjectileScript projectile in projectilePool.instantiatedObjects)
            {
                projectile.OnReturnToPool = ReturnProjectileInPool;
                newProjectilesQueue.Enqueue(projectile);
            }
            projectilesPoolsDictionnary.Add(projectilePool.elementLibraryIndex, newProjectilesQueue);
        }
    }

    #region Enemies
    public EnemySpaceShipScript GetEnemyFromPool(int enemyIndex)
    {
        EnemySpaceShipScript enemy = null;

        if (enemiesPoolsDictionnary.ContainsKey(enemyIndex))
        {
            if (enemiesPoolsDictionnary[enemyIndex].Count > 0)
                enemy = enemiesPoolsDictionnary[enemyIndex].Dequeue();
        }

        // BONUS : Gérer le manque de ressources (instantiation de nouveaux éléments)

        if (enemy != null)
            enemy.ResetPoolableObject();

        return enemy;
    }

    public void ReturnEnemyInPool(EnemySpaceShipScript enemy)
    {
        int enemyIndex = enemy.GetPoolingIndex;
        if (enemiesPoolsDictionnary.ContainsKey(enemyIndex))
            enemiesPoolsDictionnary[enemyIndex].Enqueue(enemy);
    }
    #endregion

    #region Obstacles
    public Obstacle GetObstacleFromPool(int obstacleIndex)
    {
        Obstacle obstacle = null;

        if (obstaclePoolsDictionnary.ContainsKey(obstacleIndex))
            if (obstaclePoolsDictionnary[obstacleIndex].Count > 0)
                obstacle = obstaclePoolsDictionnary[obstacleIndex].Dequeue();

        // BONUS : Gérer le manque de ressources (instantiation de nouveaux éléments)

        if (obstacle != null)
            obstacle.ResetPoolableObject();

        return obstacle;
    }

    public void ReturnObstacleInPool(Obstacle obstacle)
    {
        int obstacleIndex = obstacle.GetPoolingIndex;
        if (obstaclePoolsDictionnary.ContainsKey(obstacleIndex))
            obstaclePoolsDictionnary[obstacleIndex].Enqueue(obstacle);
    }
    #endregion

    #region Projectiles
    public ProjectileScript GetProjectileFromPool(int projectileIndex)
    {
        ProjectileScript projectile = null;

        if (projectilesPoolsDictionnary.ContainsKey(projectileIndex))
            if (projectilesPoolsDictionnary[projectileIndex].Count > 0)
                projectile = projectilesPoolsDictionnary[projectileIndex].Dequeue();

        // BONUS : Gérer le manque de ressources (instantiation de nouveaux éléments)

        if (projectile != null)
            projectile.ResetPoolableObject();

        return projectile;
    }

    public void ReturnProjectileInPool(ProjectileScript projectile)
    {
        int projectileIndex = projectile.GetPoolingIndex;
        if (projectilesPoolsDictionnary.ContainsKey(projectileIndex))
            projectilesPoolsDictionnary[projectileIndex].Enqueue(projectile);
    }
    #endregion

}

public abstract class Pool
{
    public void DestroyPoolParent()
    {
        if (poolParent != null)
            Object.DestroyImmediate(poolParent.gameObject);
    }

    public int elementLibraryIndex;
    [Range(1, 1000)] public int numberOfElements;
    public Transform poolParent;
}

[System.Serializable]
public class EnemyPool : Pool
{
    public EnemySpaceShipScript enemyPrefab;
    public List<EnemySpaceShipScript> instantiatedObjects;

    public void DestroyPoolObjects()
    {
        foreach (EnemySpaceShipScript enemy in instantiatedObjects)
            if (enemy != null) 
                Object.DestroyImmediate(enemy.gameObject);

        instantiatedObjects = new List<EnemySpaceShipScript>();
    }
}

[System.Serializable]
public class ObstaclePool : Pool
{
    public Obstacle obstaclePrefab;
    public List<Obstacle> instantiatedObjects;

    public void DestroyPoolObjects()
    {
        foreach (Obstacle obstacle in instantiatedObjects)
            if (obstacle != null)
                Object.DestroyImmediate(obstacle.gameObject);

        instantiatedObjects = new List<Obstacle>();
    }
}

[System.Serializable]
public class ProjectilePool : Pool
{
    public ProjectileScript projectilePrefab;
    public List<ProjectileScript> instantiatedObjects;

    public void DestroyPoolObjects()
    {
        foreach (ProjectileScript projectile in instantiatedObjects)
            if (projectile != null)
                Object.DestroyImmediate(projectile.gameObject);

        instantiatedObjects = new List<ProjectileScript>();
    }
}
