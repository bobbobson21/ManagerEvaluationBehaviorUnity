using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

#if UNITY_EDITOR

namespace MEBS.Editor
{
    public class MEB_UI_BehaviourEditor_ManagerData
    {
        public string m_name = "";

        public virtual MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            return new MEB_BaseBehaviourData_ItemSettings();
        }
    }


    public class MEB_UI_BehaviourEditor : EditorWindow
    {
        private List<MEB_BaseBehaviourData_ChainScopeItemWapper> m_loadedData = null;
        private MEB_BaseBehaviourData m_loadedObject = null;

        private Vector2 m_scrollPos = Vector2.zero;

        private static List<MEB_UI_BehaviourEditor_ManagerData> m_listDataNormal = new List<MEB_UI_BehaviourEditor_ManagerData>();
        private static List<MEB_UI_BehaviourEditor_ManagerData> m_listDataEval = new List<MEB_UI_BehaviourEditor_ManagerData>();

        public static void AddNormalManager(MEB_UI_BehaviourEditor_ManagerData manager)
        {
            m_listDataNormal.Add(manager);
        }

        public static void AddEvalManager(MEB_UI_BehaviourEditor_ManagerData manager)
        {
            m_listDataEval.Add(manager);
        }


        [OnOpenAsset(1)]
        public static bool OpenFileWithEditor(int fileId, int line)
        {
            try
            {
                string filePath = AssetDatabase.GetAssetPath(fileId);
                MEB_BaseBehaviourData assetData = AssetDatabase.LoadAssetAtPath<MEB_BaseBehaviourData>(filePath);

                if (assetData != null)
                {                    
                    OpenWindow(assetData);
                }
            }
            catch
            { 
            
            }

            return false;
        }

        public static void OpenWindow(MEB_BaseBehaviourData data)
        {
            MEB_UI_BehaviourEditor window = GetWindow<MEB_UI_BehaviourEditor>("MEB manager behaviour editor"); //cant have more than one
            window.position = new Rect(500, 0, Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);

            if (data.m_items == null)
            {
                data.m_items = new List<MEB_BaseBehaviourData_ChainScopeItemWapper>();
            }

            window.m_loadedData = data.m_items;
            window.m_loadedObject = data;
        }

        private MEB_BaseBehaviourData_ItemSettings RenderAddManagerField(bool displayNormalManagers, bool displayEvalManagers)
        {
            int width = 256;
            int height = 19;

            int arrayLength = 1;

            if (displayNormalManagers == true) { arrayLength += m_listDataNormal.Count; }
            if (displayEvalManagers == true) { arrayLength += m_listDataEval.Count; }

            MEB_UI_BehaviourEditor_ManagerData[] resultExacuteableList = new MEB_UI_BehaviourEditor_ManagerData[arrayLength];
            string[] resultList = new string[arrayLength];
            int writePoint = 1;

            resultExacuteableList[0] = null;
            resultList[0] = "+ (none)";

            if (displayNormalManagers == true)
            {
                for (int i = 0; i < m_listDataNormal.Count; i++)
                {
                    resultExacuteableList[writePoint] = m_listDataNormal[i];
                    resultList[writePoint] = m_listDataNormal[i].m_name;
                    writePoint++;
                }
            }

            if (displayEvalManagers == true)
            {
                for (int i = 0; i < m_listDataEval.Count; i++)
                {
                    resultExacuteableList[writePoint] = m_listDataEval[i];
                    resultList[writePoint] = m_listDataEval[i].m_name;
                    writePoint++;
                }
            }
            
            
            float oldVal = EditorStyles.popup.fixedHeight;
            EditorStyles.popup.fixedHeight = height;

            int indexOfRequestedManager = EditorGUILayout.Popup(0, resultList, GUILayout.Width(width), GUILayout.Height(height));

            EditorStyles.popup.fixedHeight = oldVal;

            if (resultExacuteableList[indexOfRequestedManager] != null)
            {
                MEB_BaseBehaviourData_ItemSettings settings = resultExacuteableList[indexOfRequestedManager].CreateInstance();
                settings.m_blackboardIdenifyers = new List<string>();
                settings.m_blackboardKeys = new List<string>();

                return settings;
            }

            return null;
        }

        private MEB_BaseBehaviourData_Item RenderAddEvalField()
        {
            int width = 44;

            if (GUILayout.Button("E+", GUILayout.Width(width)) == true)
            {
                MEB_BaseBehaviourData_Item item = new MEB_BaseBehaviourData_Item();
                item.m_isNormalManager = false;

                item.m_evalurators = new List<MEB_BaseBehaviourData_ItemSettings>();
                item.m_runAfterEval = new List<MEB_BaseBehaviourData_ItemSettings>();
                item.m_runBeforeEval = new List<MEB_BaseBehaviourData_ItemSettings>();
                item.m_useInEval = new List<MEB_BaseBehaviourData_ItemSettings>();

                return item;
            }

            return null;
        }

        private MEB_BaseBehaviourData_ChainScopeItemWapper RenderAddScopeField()
        {
            int width = 44;

            if (GUILayout.Button("{}+", GUILayout.Width(width)) == true)
            {
                MEB_BaseBehaviourData_ChainScopeItemWapper item = new MEB_BaseBehaviourData_ChainScopeItemWapper();
                item.m_items = new List<MEB_BaseBehaviourData_Item>();
                item.m_isScoped = true;

                return item;
            }

            return null;
        }

        private bool RenderManager(MEB_BaseBehaviourData_ItemSettings item)
        {
            bool returnDeleteManager = false;

            int bigSpace = 8;
            int smallSpace = 4;

            int indexWidth = 40;
            int buttonWidth = 44;

            int xPadding = 8;
            int yPadding = 4;

            GUILayout.BeginHorizontal();
            GUILayout.Space(xPadding);

            GUILayout.BeginVertical(EditorStyles.textArea);

            GUILayout.BeginHorizontal();
            GUILayout.Label("name: " + item.m_displayName, MEB_GUI_Styles.TitleTextStyle());

            GUILayout.FlexibleSpace();

            if (item.m_runtimeManager != null && Application.isPlaying == true)
            {
                EditorGUILayout.Toggle(item.m_runtimeManager.IsAllowedToExecute(), GUILayout.Width(22));
            }

            if (GUILayout.Button("delete", GUILayout.Width(buttonWidth *2)) == true)
            {
                returnDeleteManager = true;
            }
             GUILayout.EndHorizontal();

            GUILayout.Space(smallSpace);
            GUILayout.Label(item.m_displayDiscription, MEB_GUI_Styles.NormalTextStyle());

            GUILayout.Space(bigSpace);

            GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
            item.m_displayBlackboardSettingExpanded = EditorGUILayout.Foldout(item.m_displayBlackboardSettingExpanded, "blackboard values");

            if (item.m_displayBlackboardSettingExpanded == true)
            {
                GUILayout.Label("index: idenifyer as text, key as text");
                GUILayout.Space(smallSpace);

                for (int i = 0; i < item.m_blackboardKeys.Count; i++) //renders keys
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{i}: ", GUILayout.Width(indexWidth));
                    item.m_blackboardIdenifyers[i] = EditorGUILayout.TextField(item.m_blackboardIdenifyers[i]);
                    item.m_blackboardKeys[i] = EditorGUILayout.TextField(item.m_blackboardKeys[i]);
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal(); //renders add and remove buttons
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("+", GUILayout.Width(buttonWidth)) == true)
                {
                    item.m_blackboardIdenifyers.Add(""); //add nothing let the user fill it out
                    item.m_blackboardKeys.Add("");
                }

                if (GUILayout.Button("-", GUILayout.Width(buttonWidth)) == true && item.m_blackboardKeys.Count > 0)
                {
                    int indexToRemove = item.m_blackboardIdenifyers.Count - 1;

                    item.m_blackboardIdenifyers.RemoveAt(indexToRemove);
                    item.m_blackboardKeys.RemoveAt(indexToRemove);
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical(); //end of blackbord settings

            item.OnGUI();

            GUILayout.EndVertical();

            GUILayout.Space(xPadding);
            GUILayout.EndHorizontal();

            GUILayout.Space(yPadding);

            return returnDeleteManager;
        }

        private bool RenderEvalScope(MEB_BaseBehaviourData_Item item)
        {
            bool returnDeleteManager = false;

            int buttonWidth = 44;
            int xPadding = 8;
            int yPadding = 8;

            GUILayout.BeginHorizontal();
            GUILayout.Space(xPadding);

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("delete", GUILayout.Width(buttonWidth * 2)) == true)
            {
                returnDeleteManager = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(0));
            GUILayout.Label("before evaluration", EditorStyles.boldLabel); //before eval
            MEB_BaseBehaviourData_ItemSettings managerData = RenderAddManagerField(true, false);

            if (managerData != null)
            {
                item.m_runBeforeEval.Add( managerData );
            }

            for (int i = 0; i < item.m_runBeforeEval.Count; i++)
            {
                if (RenderManager(item.m_runBeforeEval[i]) == true)
                {
                    item.m_runBeforeEval.RemoveAt(i);
                }
            }
            GUILayout.EndVertical();//section divide

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(2));
            GUILayout.Label("evaluators", EditorStyles.boldLabel); //eval
            managerData = RenderAddManagerField(false, true);

            if (managerData != null)
            {
                item.m_evalurators.Add(managerData);
            }

            for (int i = 0; i < item.m_evalurators.Count; i++)
            {
                if (RenderManager(item.m_evalurators[i]) == true)
                {
                    item.m_evalurators.RemoveAt(i);
                }
            }
            GUILayout.EndVertical(); //section divide

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(2));
            GUILayout.Label("managers to evalurate", EditorStyles.boldLabel); //eval data
            managerData = RenderAddManagerField(true, false);

            if (managerData != null)
            {
                item.m_useInEval.Add(managerData);
            }

            for (int i = 0; i < item.m_useInEval.Count; i++)
            {
                if (RenderManager(item.m_useInEval[i]) == true)
                {
                    item.m_useInEval.RemoveAt(i);
                }
            }
            GUILayout.EndVertical(); //section divide

            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(2));
            GUILayout.Label("after evaluration", EditorStyles.boldLabel); //after eval
            managerData = RenderAddManagerField(true, false);

            if (managerData != null)
            {
                item.m_runAfterEval.Add(managerData);
            }

            for (int i = 0; i < item.m_runAfterEval.Count; i++)
            {
                if (RenderManager(item.m_runAfterEval[i]) == true)
                {
                    item.m_runAfterEval.RemoveAt(i);
                }
            }

            GUILayout.EndVertical(); //section divide
            GUILayout.EndVertical();

            GUILayout.Space(xPadding);
            GUILayout.EndHorizontal();

            GUILayout.Space(yPadding);

            return returnDeleteManager;
        }

        private bool RenderScopeStart(MEB_BaseBehaviourData_ChainScopeItemWapper item)
        {
            bool returnDeleteManager = false;
            int buttonWidth = 44;

            int xPadding = 8;
            int yPadding = 8;

            GUILayout.BeginHorizontal();
            GUILayout.Space(xPadding);

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();
            MEB_BaseBehaviourData_ItemSettings managerData = RenderAddManagerField(true, false);
            MEB_BaseBehaviourData_Item evalData = RenderAddEvalField();

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("delete", GUILayout.Width(buttonWidth * 2)) == true)
            {
                returnDeleteManager = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(yPadding);


            if (managerData != null)
            {
                item.m_items.Add(new MEB_BaseBehaviourData_Item());
                item.m_items[item.m_items.Count -1].m_noneEvalurationManager = managerData;
            }

            if (evalData != null)
            {
                item.m_items.Add(evalData);
            }

            return returnDeleteManager;
        }

        private void RenderScopeEnd()
        {
            int xPadding = 8;
            int yPadding = 8;

            GUILayout.EndVertical();
            GUILayout.Space(xPadding);
            GUILayout.EndHorizontal();

            GUILayout.Space(yPadding);
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            MEB_BaseBehaviourData_ItemSettings managerData = RenderAddManagerField(true, false);
            MEB_BaseBehaviourData_Item evalData = RenderAddEvalField();
            MEB_BaseBehaviourData_ChainScopeItemWapper scopeData = RenderAddScopeField();
            GUILayout.EndHorizontal();

            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);

            if (managerData != null)
            {
                m_loadedData.Add(new MEB_BaseBehaviourData_ChainScopeItemWapper());

                m_loadedData[m_loadedData.Count - 1].m_items = new List<MEB_BaseBehaviourData_Item>{ new MEB_BaseBehaviourData_Item() };
                m_loadedData[m_loadedData.Count - 1].m_items[0].m_noneEvalurationManager = managerData;
            }

            if (evalData != null)
            {
                m_loadedData.Add(new MEB_BaseBehaviourData_ChainScopeItemWapper());

                m_loadedData[m_loadedData.Count - 1].m_items = new List<MEB_BaseBehaviourData_Item> { evalData };
            }

            if (scopeData != null)
            {
                m_loadedData.Add(scopeData);
            }

            for (int i = 0; i < m_loadedData.Count; i++)
            {
                bool deleteScope = false;

                if (m_loadedData[i].m_isScoped == true)
                {
                    deleteScope = RenderScopeStart(m_loadedData[i]);
                }

                for (int j = 0; j < m_loadedData[i].m_items.Count; j++)
                {
                    if (m_loadedData[i].m_items[j].m_isNormalManager == true)
                    {
                        if (RenderManager(m_loadedData[i].m_items[j].m_noneEvalurationManager) == true)
                        {
                            m_loadedData[i].m_items.RemoveAt(j);

                            if (m_loadedData[i].m_isScoped == false)
                            { 
                                m_loadedData.RemoveAt(i);
                                EditorGUILayout.EndScrollView();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (RenderEvalScope(m_loadedData[i].m_items[j]) == true)
                        {
                            m_loadedData[i].m_items.RemoveAt(j);

                            if (m_loadedData[i].m_isScoped == false)
                            {
                                m_loadedData.RemoveAt(i);
                                EditorGUILayout.EndScrollView();
                                return;
                            }
                        }
                    }
                }

                if (m_loadedData[i].m_isScoped == true)
                {
                    RenderScopeEnd();

                    if (deleteScope == true)
                    {
                        m_loadedData.RemoveAt(i);
                        EditorGUILayout.EndScrollView();
                        return;
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void OnDestroy()
        {
            EditorUtility.SetDirty(m_loadedObject);
            AssetDatabase.SaveAssetIfDirty(m_loadedObject);
        }
    }
}

#endif