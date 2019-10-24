using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyCreationWindow : EditorWindow
{
    [MenuItem("Window/DO A BARELL ROLL/Enemy Creation Window", priority = 0)]
    public static void OpenWindow()
    {
        OpenWindow(null);
    }

    public static void OpenWindow(string folderPath)
    {
        EnemyCreationWindow window = EnemyCreationWindow.GetWindow(typeof(EnemyCreationWindow)) as EnemyCreationWindow;
        window.Init(folderPath);
    }

    DefaultAsset selectedFolderRef;
    string newEnemyName = "NewEnemy";
    string newWeaponName = "NewEnemy";
    bool createFolder = true;

    EnemyCreationParameters enemyCreationParameters = new EnemyCreationParameters(true);

    bool createLinkedWeaponParameters = true;
    bool createLinkedWeaponObject = true;
    WeaponCreationParameters newWeaponCreationParameters = new WeaponCreationParameters(true);
    public void Init(string folderPath)
    {
        Show();
        selectedFolderRef = AssetDatabase.LoadAssetAtPath(folderPath, typeof(DefaultAsset)) as DefaultAsset;
        createFolder = true;
    }

    private void OnGUI()
    {
        EditorStaticMethods.ShowFolderAndAskIfCreateNew(ref selectedFolderRef, ref createFolder);

        float oldWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth *= 1.5f;

        GUILayout.Space(8);
        GUILayout.Label("Enemy Parameters", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        newEnemyName = EditorGUILayout.TextField("New Enemy Name", newEnemyName);
        if(EditorGUI.EndChangeCheck())
            newWeaponName = newEnemyName + " Weapon";

        enemyCreationParameters.enemyIdentifyingColor = EditorGUILayout.ColorField("Enemy Identifying Color", enemyCreationParameters.enemyIdentifyingColor);

        enemyCreationParameters.lifeAmount = EditorGUILayout.IntField("Life Amount", enemyCreationParameters.lifeAmount);
        enemyCreationParameters.aimingType = (EnemyAimingType)EditorGUILayout.EnumPopup(new GUIContent("Aiming Type"), enemyCreationParameters.aimingType);

        GUILayout.Space(8);
        GUILayout.Label("Linked Weapon", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        createLinkedWeaponParameters =  EditorGUILayout.Toggle("Create Linked Weapon Parameters", createLinkedWeaponParameters);
        if (EditorGUI.EndChangeCheck())
            newWeaponName = newEnemyName + " Weapon";

        if (createLinkedWeaponParameters)
        {
            EditorGUI.indentLevel++;
            GUILayout.BeginVertical("box");
            WeaponSetCreationWindow.ShowWeaponSetCreationParameters(selectedFolderRef, ref newWeaponName, false, ref createLinkedWeaponObject, ref newWeaponCreationParameters);
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
        }

        GUILayout.Space(16);
        if (selectedFolderRef != null)
        {
            if (GUILayout.Button("Create the new Enemy : \"" + newEnemyName + "\" !"))
            {
                string finalFolderPath = selectedFolderRef.GetFolderPath();

                if (createFolder)
                {
                    string folderCreationPath = finalFolderPath;
                    finalFolderPath = AssetDatabase.GenerateUniqueAssetPath(finalFolderPath + "/" + newEnemyName + " Set");
                    AssetDatabase.CreateFolder(folderCreationPath, newEnemyName + " Set");
                }

                EnemySpaceShipScript newEnemyTempObj = EnemyCreationParameters.ComposeEnemy(enemyCreationParameters, newEnemyName);
                EnemySpaceShipScript newEnemyPrefab = EditorStaticMethods.CreateEnemyPrefabInFolder(finalFolderPath, newEnemyTempObj);

                if(newEnemyPrefab == null)
                {
                    Debug.LogError("Couldn't create enemy Set");
                    return;
                }

                WeaponScript newWeaponPrefab = null;
                if (createLinkedWeaponObject)
                {
                    WeaponScript newWeapon = WeaponCreationParameters.ComposeWeapon(newWeaponCreationParameters, newWeaponName);
                    newWeaponPrefab = EditorStaticMethods.CreateWeaponObjectInFolder(finalFolderPath, newWeapon);
                }

                WeaponParameters newWeaponParameters = null;
                if (newWeaponPrefab != null)
                    newWeaponParameters = EditorStaticMethods.CreateWeaponSetInFolder(finalFolderPath, newWeaponName, newWeaponPrefab);
                else
                    newWeaponParameters = EditorStaticMethods.CreateWeaponSetInFolder(finalFolderPath, newWeaponName);

                newEnemyPrefab.GetShootingSystem.SetWeaponParameters(newWeaponParameters);

                Selection.activeObject = newEnemyPrefab;
                EditorGUIUtility.PingObject(newEnemyPrefab);

                #region Library
                //LevelPrefabsLibrary enemiesLibrary = AssetDatabase.LoadAssetAtPath("Assets/Resources/Level Prefabs Library.asset", typeof(LevelPrefabsLibrary)) as LevelPrefabsLibrary;
                ScriptableObject library = Resources.Load("Level Prefabs Library") as ScriptableObject;
                LevelPrefabsLibrary prefabsLibrary = library as LevelPrefabsLibrary;

                if (prefabsLibrary == null)
                {
                    Debug.LogError("Library Not Found");
                    return;
                }

                prefabsLibrary.AddEnemyPrefabInformations(newEnemyPrefab.gameObject, enemyCreationParameters.enemyIdentifyingColor);
                EditorUtility.SetDirty(prefabsLibrary);
                #endregion
            }
        }

        EditorGUIUtility.labelWidth = oldWidth;
    }
}

public struct EnemyCreationParameters
{
    public EnemyCreationParameters(bool ph)
    {
        lifeAmount = 50;
        aimingType = EnemyAimingType.NoAiming;
        enemyIdentifyingColor = Color.magenta;
    }

    public int lifeAmount;
    public EnemyAimingType aimingType;
    public Color enemyIdentifyingColor;

    public static EnemySpaceShipScript ComposeEnemy(EnemyCreationParameters weaponCreationParameters, string enemyName)
    {
        GameObject baseEnemyPrefab = AssetDatabase.LoadAssetAtPath("Assets/Enemies/BaseEnemyPrefab.prefab", typeof(GameObject)) as GameObject;

        if (baseEnemyPrefab == null) 
        {
            Debug.LogError("BASE ENEMY PREFAB NOT FOUND : Make sure to have a \"BaseEnemyPrefab\" prefab in the \"Enemies\" folder");
            return null; 
        }

        GameObject newEnemyObject = Object.Instantiate(baseEnemyPrefab);
        newEnemyObject.name = enemyName;

        EnemySpaceShipScript enemyComponent = newEnemyObject.GetComponent<EnemySpaceShipScript>();
        if (enemyComponent == null)
        {
            Debug.LogError("No EnemySpaceShipScript affected on the found object");
            return null;
        }

        enemyComponent.GetRelatedDamageableComponent.SetMaxLifeAmount(weaponCreationParameters.lifeAmount);
        enemyComponent.SetAimingType(weaponCreationParameters.aimingType);

        return enemyComponent;
    }
}