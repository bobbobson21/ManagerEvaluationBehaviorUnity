#if UNITY_EDITOR

using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class MEB_CreateManager
{
    [MenuItem("Assets/Create/MEB/create manager script", priority = 4)]
    public static void CreateManager()
    {
        string pathOfNewFile = AssetDatabase.GetAssetPath(Selection.activeObject); //finds out where to put the new blackboard
        string nameOfNewFile = "MEB ManagerScript";
        string extensionOfFile = ".cs";

        int fileIndex = 1;

        while (File.Exists(pathOfNewFile + "/" + nameOfNewFile + extensionOfFile) == true) //what should be the blackboards name
        {
            if (File.Exists(pathOfNewFile + "/" + nameOfNewFile + $" ({fileIndex})" + extensionOfFile) == false)
            {
                nameOfNewFile = nameOfNewFile + $" ({fileIndex})";

                break;
            }

            fileIndex++;
        }

        string id = (pathOfNewFile + "/" + nameOfNewFile + extensionOfFile).GetHashCode().ToString(); //makes id
        id = id.Replace("1", "A");
        id = id.Replace("2", "B");
        id = id.Replace("3", "C");
        id = id.Replace("4", "D");
        id = id.Replace("5", "E");
        id = id.Replace("6", "F");
        id = id.Replace("7", "G");
        id = id.Replace("8", "H");
        id = id.Replace("9", "I");
        id = id.Replace("0", "J");
        id = id.Replace("-", "__");

        string exportPath = "Assets/MEBS/Edit/Menus/ExampleManagerContainer.txt";
        string exportFileDataRaw = ((TextAsset)EditorGUIUtility.Load(exportPath)).text; //loads file contents and makes new class
        exportFileDataRaw = exportFileDataRaw.Replace("[IDHere]", id);


        File.WriteAllText(pathOfNewFile + "/" + nameOfNewFile + extensionOfFile, exportFileDataRaw);
        AssetDatabase.ImportAsset(pathOfNewFile + "/" + nameOfNewFile + extensionOfFile);
    }
}

#endif