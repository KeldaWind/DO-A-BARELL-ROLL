using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorStaticMethods
{
    #region Folder Management
    public static void ShowFolderAndAskIfCreateNew(ref DefaultAsset selectedFolderRef, ref bool createFolder)
    {
        selectedFolderRef = EditorGUILayout.ObjectField("Target Folder", selectedFolderRef, typeof(DefaultAsset), false) as DefaultAsset;
        createFolder = EditorGUILayout.Toggle("Create New Folder", createFolder);
    }
    #endregion

    #region Weapons Creation
    [MenuItem("Assets/Create/DO A BARREL ROLL Creation Tools/Create Weapon Set", priority = 0)]
    public static void CreateWeaponSetInSelectedFolder()
    {
        Object selectedObject = Selection.activeObject;

        string weaponCreationPath = selectedObject.GetFolderPath();

        WeaponSetCreationWindow.OpenWindow(weaponCreationPath);
    }

    public static WeaponParameters CreateWeaponSetInFolder(string folderPath, string weaponName)
    {
        WeaponScript baseWeaponPrefab = AssetDatabase.LoadAssetAtPath<WeaponScript>("Assets/Weapons/BaseWeapon.prefab");
        return CreateWeaponSetInFolder(folderPath, weaponName, baseWeaponPrefab);        
    }

    public static WeaponParameters CreateWeaponSetInFolder(string folderPath, string weaponName, WeaponScript linkedWeapon)
    {
        WeaponParameters weaponParams = ScriptableObject.CreateInstance<WeaponParameters>();
        ShootParameters shootParams = ScriptableObject.CreateInstance<ShootParameters>();
        ProjectileParameters projectileParams = ScriptableObject.CreateInstance<ProjectileParameters>();
        
        weaponParams.SetWeaponPrefab(linkedWeapon);

        weaponParams.SetShootParameters(shootParams);

        ProjectileScript baseProjectilePrefab = AssetDatabase.LoadAssetAtPath<ProjectileScript>("Assets/Weapons/BaseProjectile.prefab");
        shootParams.SetProjectilePrefab(baseProjectilePrefab);

        shootParams.SetProjectileParameters(projectileParams);

        string weaponPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/" + weaponName + " Weapon Parameters.asset");
        string shootPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/" + weaponName + " Shoot Parameters.asset");
        string projectilePath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/" + weaponName + " Projectiles Parameters.asset");

        AssetDatabase.CreateAsset(weaponParams, weaponPath);
        AssetDatabase.CreateAsset(shootParams, shootPath);
        AssetDatabase.CreateAsset(projectileParams, projectilePath);

        EditorUtility.SetDirty(weaponParams);
        EditorUtility.SetDirty(shootParams);
        EditorUtility.SetDirty(projectileParams);

        return weaponParams;
    }

    public static WeaponScript CreateWeaponObjectInFolder(string folderPath, WeaponScript weapon)
    {
        string weaponPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/" + weapon.name + ".prefab");
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(weapon.gameObject, weaponPath);
        EditorUtility.SetDirty(prefab);
        Object.DestroyImmediate(weapon.gameObject);

        return prefab.GetComponent<WeaponScript>();
    }
    #endregion

    #region Enemies Creation
    [MenuItem("Assets/Create/DO A BARREL ROLL Creation Tools/Create Enemy", priority = 0)]
    public static void CreateEnemyInSelectedFolder()
    {
        Object selectedObject = Selection.activeObject;

        string enemyCreationPath = selectedObject.GetFolderPath();

        EnemyCreationWindow.OpenWindow(enemyCreationPath);
    }

    public static EnemySpaceShipScript CreateEnemyPrefabInFolder(string folderPath, EnemySpaceShipScript enemy)
    {
        string enemyPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/" + enemy.name + ".prefab");
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(enemy.gameObject, enemyPath);
        EditorUtility.SetDirty(prefab);
        Object.DestroyImmediate(enemy.gameObject);

        return prefab.GetComponent<EnemySpaceShipScript>();
    }
    #endregion

    #region Quests Creation
    [MenuItem("Assets/Create/DO A BARREL ROLL Creation Tools/Create Quests", priority = 0)]
    public static void CreateQuestsInSelectedFolder()
    {
        Object selectedObject = Selection.activeObject;

        string enemyCreationPath = selectedObject.GetFolderPath();

        QuestsCreationWindow.OpenWindow(enemyCreationPath);
    }

    public static void CreateOrReplaceQuestInFolder(string folderPath, Quest quest)
    {
        //string questPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/" + quest.questName + ".asset");
        string questPath = folderPath + "/" + quest.questName + ".asset";

        Quest questAtPath = AssetDatabase.LoadAssetAtPath(questPath, typeof(Quest)) as Quest;

        if (questAtPath != null)
        {
            questAtPath.questName = quest.questName;
            questAtPath.questDescription = quest.questDescription;
            questAtPath.questType = quest.questType;
            questAtPath.questRealisationType = quest.questRealisationType;
            questAtPath.valueToReach = quest.valueToReach;

            EditorUtility.SetDirty(questAtPath);
        }
        else
        {
            AssetDatabase.CreateAsset(quest, questPath);
            EditorUtility.SetDirty(quest);
        }
    }
    #endregion

    /*#region Level Chunks Creation
    [MenuItem("Assets/Create/DO A BARREL ROLL/Create Level Chunk", priority = 0)]
    public static void CreateLevelChunkInSelectedFolder()
    {
        Object selectedObject = Selection.activeObject;

        string levelChunkCreationPath = selectedObject.GetFolderPath();

        LevelChunkCreationWindow.OpenWindow(levelChunkCreationPath);
    }//
    #endregion*/

    public static Rect GetIndentedControlRect(float indentValue)
    {
        Rect indentedRect = EditorGUILayout.GetControlRect();
        indentedRect.x += indentValue;
        indentedRect.width -= indentValue;

        return indentedRect;
    }

    public static void DrawLine(float lineWidth, Color color)
    {
        Rect line = EditorGUILayout.GetControlRect();
        line.height  = lineWidth;

        EditorGUI.DrawRect(line, color);
    }

    public static bool CheckIfMouseInRect(Rect targetRect)
    {
        bool inRectangle = false;

        Vector2 mousePos = Event.current.mousePosition;

        Vector2 bottomLeftRectPos = new Vector2(targetRect.x + targetRect.width, targetRect.y + targetRect.height);

        if (mousePos.x > targetRect.x && mousePos.x < bottomLeftRectPos.x && mousePos.y > targetRect.y && mousePos.y < bottomLeftRectPos.y)
            inRectangle = true;

        return inRectangle;
    }
}
