using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WeaponSetCreationWindow : EditorWindow
{
    [MenuItem("Window/DO A BARELL ROLL/Weapon Set Creation Window", priority = 0)]
    public static void OpenWindow()
    {
        OpenWindow(null);
    }

    public static void OpenWindow(string folderPath)
    {
        WeaponSetCreationWindow window = EditorWindow.GetWindow(typeof(WeaponSetCreationWindow)) as WeaponSetCreationWindow;
        window.Init(folderPath);
    }

    DefaultAsset selectedFolderRef;
    string newWeaponName = "NewWeapon";
    bool createFolder = true;

    bool createLinkedObject;
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

        GUILayout.Space(12);

        float oldWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth *= 1.5f;
        ShowWeaponSetCreationParameters(selectedFolderRef, ref newWeaponName, createFolder, ref createLinkedObject, ref newWeaponCreationParameters);
        EditorGUIUtility.labelWidth = oldWidth;

        if (selectedFolderRef != null)
        {
            if (GUILayout.Button("Create the new Weapon : \"" + newWeaponName + "\" !"))
            {
                string finalFolderPath = selectedFolderRef.GetFolderPath();

                if (createFolder)
                {
                    string folderCreationPath = finalFolderPath;
                    finalFolderPath = AssetDatabase.GenerateUniqueAssetPath(finalFolderPath + "/" + newWeaponName + " Set");
                    
                    Debug.Log("Create Folder \"" + newWeaponName + "Set" + "\" in \"" + folderCreationPath + "\"");
                    AssetDatabase.CreateFolder(folderCreationPath, newWeaponName + " Set");
                }

                WeaponScript newWeaponPrefab = null;
                if (createLinkedObject)
                {
                    WeaponScript newWeapon = WeaponCreationParameters.ComposeWeapon(newWeaponCreationParameters, newWeaponName);
                    newWeaponPrefab = EditorStaticMethods.CreateWeaponObjectInFolder(finalFolderPath, newWeapon);
                }


                WeaponParameters newWeaponParameters = null;
                if (newWeaponPrefab != null)
                    newWeaponParameters = EditorStaticMethods.CreateWeaponSetInFolder(finalFolderPath, newWeaponName, newWeaponPrefab);
                else
                    newWeaponParameters = EditorStaticMethods.CreateWeaponSetInFolder(finalFolderPath, newWeaponName);

                if (newWeaponPrefab != null)
                {
                    Selection.activeObject = newWeaponPrefab;
                    EditorGUIUtility.PingObject(newWeaponPrefab);
                    // PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(newWeaponPrefab.GetInstanceID()));
                }
                else if(newWeaponParameters != null)
                {
                    Selection.activeObject = newWeaponParameters;
                    EditorGUIUtility.PingObject(newWeaponParameters);
                }
            }
        }
    }

    public static void ShowWeaponSetCreationParameters(DefaultAsset selectedFolderRef, ref string newWeaponName, bool createFolder, ref bool createLinkedObject, ref WeaponCreationParameters weaponCreationParameters)
    {        
        EditorGUILayout.LabelField("Weapon Name", EditorStyles.boldLabel);
        newWeaponName = EditorGUILayout.TextField("New Weapon Name", newWeaponName);

        GUILayout.Space(12);

        EditorGUILayout.LabelField("Weapon Object", EditorStyles.boldLabel);
        createLinkedObject = EditorGUILayout.Toggle("Create Weapon Object", createLinkedObject);
        if (createLinkedObject)
        {
            weaponCreationParameters.numberOfShootOrigins = EditorGUILayout.IntField("Number of Shoot Origin", weaponCreationParameters.numberOfShootOrigins);
            weaponCreationParameters.lateralDistanceBetweenEachOrigin = EditorGUILayout.FloatField("Lateral Distance between each Origin", weaponCreationParameters.lateralDistanceBetweenEachOrigin);
            weaponCreationParameters.angleBetweenEachOrigin = EditorGUILayout.FloatField("Angle between each Origin", weaponCreationParameters.angleBetweenEachOrigin);
        }

        GUILayout.Space(12);
    }
}

public struct WeaponCreationParameters
{
    public WeaponCreationParameters(bool ph)
    {
        numberOfShootOrigins = 1;
        lateralDistanceBetweenEachOrigin = 0.5f;
        angleBetweenEachOrigin = 0;
    }

    public int numberOfShootOrigins;
    public float lateralDistanceBetweenEachOrigin;
    public float angleBetweenEachOrigin;

    public static WeaponScript ComposeWeapon(WeaponCreationParameters weaponCreationParameters, string weaponName)
    {
        GameObject newWeaponObject = new GameObject();
        newWeaponObject.name = weaponName;

        WeaponScript weaponComponent = newWeaponObject.AddComponent<WeaponScript>();

        List<ShootOriginScript> origins = new List<ShootOriginScript>();
        for (int i = 0; i < weaponCreationParameters.numberOfShootOrigins; i++)
        {
            GameObject newOriginObject = new GameObject();
            newOriginObject.name = "Shoot Origin " + (i + 1);
            newOriginObject.transform.parent = newWeaponObject.transform;
            newOriginObject.transform.localPosition = Vector3.zero;
            newOriginObject.transform.localRotation = Quaternion.identity;

            ShootOriginScript shootOriginComponent = newOriginObject.AddComponent<ShootOriginScript>();

            float xPos = (-weaponCreationParameters.lateralDistanceBetweenEachOrigin * (weaponCreationParameters.numberOfShootOrigins - 1))/2 + i * weaponCreationParameters.lateralDistanceBetweenEachOrigin;
            newOriginObject.transform.localPosition = new Vector3(xPos, 0, 0);

            float zAngle = (weaponCreationParameters.angleBetweenEachOrigin * (weaponCreationParameters.numberOfShootOrigins - 1)) / 2 - i * weaponCreationParameters.angleBetweenEachOrigin;
            newOriginObject.transform.localRotation = Quaternion.Euler(0, 0, zAngle);
         
            origins.Add(shootOriginComponent);
        }

        weaponComponent.SetShootOrigins(origins);

        return weaponComponent;
    }
}