using MEBS.Runtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

#if UNITY_EDITOR

namespace MEBS.Editor
{
    public enum MEB_UI_BlackBoardEditor_ObjectType
    {
        numInt,
        numFloat,
        numDouble,
        spaceVector2,
        spaceVector3,
        spaceRoatation,
        logicBool,
        textChar,
        textString,
        ObjectUnknown,
    }

    public class MEB_UI_BlackBoardEditor_ObjectDetails
    { 
        public MEB_UI_BlackBoardEditor_ObjectType m_type = MEB_UI_BlackBoardEditor_ObjectType.ObjectUnknown;
        public string m_id = "";
        public object m_data = null;
    }

    public class MEB_UI_BlackBoardEditor : EditorWindow
    {
        private string m_className = "";
        private List<MEB_UI_BlackBoardEditor_ObjectDetails> m_objectDetails = new List<MEB_UI_BlackBoardEditor_ObjectDetails>();

        [OnOpenAsset(1)]
        public static bool OpenFileWithEditor(int fileId, int line)
        {
            try
            {
                string filePath = AssetDatabase.GetAssetPath(fileId);
                TextAsset assetData = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);

                if (assetData != null && assetData.text.Contains("MEB_BLACKBOARD_UI_INTERACTOR") == true)
                {
                    OpenWindow(assetData);
                }
            }
            catch
            {

            }

            return false;
        }

        public static void OpenWindow(TextAsset data)
        {
            MEB_UI_BlackBoardEditor window = GetWindow<MEB_UI_BlackBoardEditor>("MEB blackboard editor"); //cant have more than one

            window.position = new Rect(500, 0, Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);
            window.LoadAssest(data.text);

        }

        void LoadAssest(string text)
        {
            if (text.Contains("#if MEB_BLACKBOARD_UI_INTERACTOR") == true)
            {
                int startPoint = text.IndexOf("#if MEB_BLACKBOARD_UI_INTERACTOR");
                int endPoint = text.IndexOf("#if MEB_BLACKBOARD_UI_INTERACTOR");

                string result = text.Substring(startPoint, endPoint);
                string[] items = result.Split(";");

                for (int i = 0; i < items.Length; i++)
                {
                    
                }
            }
        }
    }
}

#endif