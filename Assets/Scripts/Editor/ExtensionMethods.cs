using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ExtensionMethods 
{    public static string GetFolderPath(this Object obj)
    {
        return GetFolderPath(obj, false);
    }

    public static string GetFolderPath(this Object obj, bool withLastSlash)
    {
        System.Type selectedObjectType = obj.GetType();

        string folderPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());

        if (selectedObjectType == typeof(DefaultAsset))
        {
            if (withLastSlash)
                folderPath += "/";
        }
        else
        {
            string[] folders = folderPath.Split(new char[] { '/' });
            folderPath = "";
            for (int i = 0; i < folders.Length - 1; i++)
            {
                folderPath += folders[i];

                if (i < folders.Length - 2)
                    folderPath += "/";
                else if (withLastSlash)
                    folderPath += "/";
            }
        }

        return folderPath;
    }
}
