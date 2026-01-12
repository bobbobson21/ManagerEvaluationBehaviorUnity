using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using static UnityEditor.Progress;

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
        private MEB_BaseBehaviourData m_loadedObject = null;
        private Vector2 m_scrollPos = Vector2.zero;

        private static List<MEB_UI_BehaviourEditor_ManagerData> m_listDataNormalScoped = new List<MEB_UI_BehaviourEditor_ManagerData>();
        private static List<MEB_UI_BehaviourEditor_ManagerData> m_listDataNormal = new List<MEB_UI_BehaviourEditor_ManagerData>();
        private static List<MEB_UI_BehaviourEditor_ManagerData> m_listDataEval = new List<MEB_UI_BehaviourEditor_ManagerData>();

        private float m_refreshRateMax = 0.1f;
        private float m_currentRefreshRatePoint = 0;

        public static void AddNormalManagerWithScopeRequirement(MEB_UI_BehaviourEditor_ManagerData manager)
        {
            m_listDataNormalScoped.Add(manager);
        }

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
            MEB_UI_BehaviourEditor window = GetWindow<MEB_UI_BehaviourEditor>("MEB manager evaluation behaviour editor"); //cant have more than one

            if (window.m_loadedObject == null)
            {
                window.position = new Rect(500, 0, Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);
            }

            if (data.m_items == null)
            {
                data.m_items = new List<MEB_BaseBehaviourData_ChainScopeItemWapper>();
            }

            window.m_loadedObject = data;
        }

        private MEB_BaseBehaviourData_ItemSettings RenderAddManagerField(bool displayNormalManagers,  bool displayNormalManagersScoped, bool displayEvalManagers)
        {
            if (Application.isPlaying == true) { return null; }

            int width = 256;
            int height = 19;

            int arrayLength = 1;
            if (displayNormalManagers == true) { arrayLength += m_listDataNormal.Count; }
            if (displayNormalManagersScoped == true) {arrayLength += m_listDataNormalScoped.Count; }
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

            if (displayNormalManagersScoped == true)
            {
                for (int i = 0; i < m_listDataNormalScoped.Count; i++)
                {
                    resultExacuteableList[writePoint] = m_listDataNormalScoped[i];
                    resultList[writePoint] = m_listDataNormalScoped[i].m_name;
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
            if (Application.isPlaying == true) { return null; }

            int width = 44;

            if (GUILayout.Button("E+", GUILayout.Width(width)) == true)
            {
                MEB_BaseBehaviourData_Item item = new MEB_BaseBehaviourData_Item();
                item.m_isNormalManager = false;

                item.m_evalurators = new List<MEB_BaseBehaviourData_ItemSettings>();
                item.m_useInEval = new List<MEB_BaseBehaviourData_ItemSettings>();

                return item;
            }

            return null;
        }

        private MEB_BaseBehaviourData_ChainScopeItemWapper RenderAddScopeField()
        {
            if (Application.isPlaying == true) { return null; }

            int width = 44;
            int widthButtonMain = 132;

            if (GUILayout.Button("{}+", GUILayout.Width(width)) == true)
            {
                MEB_BaseBehaviourData_ChainScopeItemWapper item = new MEB_BaseBehaviourData_ChainScopeItemWapper();
                item.m_items = new List<MEB_BaseBehaviourData_Item>();
                item.m_isScoped = true;

                return item;
            }

            if (GUILayout.Button("{}+ (main thread)", GUILayout.Width(widthButtonMain)) == true)
            {
                MEB_BaseBehaviourData_ChainScopeItemWapper item = new MEB_BaseBehaviourData_ChainScopeItemWapper();
                item.m_items = new List<MEB_BaseBehaviourData_Item>();
                item.m_isScoped = true;
                item.m_isForMainThread = true;

                return item;
            }

            return null;
        }

        private int RenderScopeIndexField(int itemIndex)
        {
            if (Application.isPlaying == true) { return itemIndex; }

            int indexButtonWidth = 120;
            int indexButtonHeight = 19;
            int tempIndex = 0;

            int.TryParse(EditorGUILayout.TextField(itemIndex.ToString(), EditorStyles.helpBox, GUILayout.Width(indexButtonWidth), GUILayout.Height(indexButtonHeight), GUILayout.MinHeight(indexButtonHeight)), out tempIndex);

            if (tempIndex >= 0)
            {
                itemIndex = tempIndex;
            }

            return itemIndex;
        }

        private int RenderDeleteField(int itemIndex)
        {
            if (Application.isPlaying == true) { return itemIndex; }

            int buttonWidth = 88;

            if (GUILayout.Button("delete", GUILayout.Width(buttonWidth)) == true)
            {
                itemIndex = -1;
            }

            return itemIndex;
        }

        private int RenderManager(MEB_BaseBehaviourData_ItemSettings item, int itemIndex)
        {
            int bigSpace = 8;
            int smallSpace = 4;

            int arrayindexWidth = 40;
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

            itemIndex = RenderScopeIndexField(itemIndex);
            itemIndex = RenderDeleteField(itemIndex);

             GUILayout.EndHorizontal();

            GUILayout.Space(smallSpace);
            GUILayout.Label(item.m_displayDiscription, MEB_GUI_Styles.NormalTextStyle());

            if (Application.isPlaying == false)
            {
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
                        GUILayout.Label($"{i}: ", GUILayout.Width(arrayindexWidth));
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
            }
            else 
            {
                item.OnGUI();
                GUILayout.Space(yPadding);
            }

            GUILayout.EndVertical();
            

            GUILayout.Space(xPadding);
            GUILayout.EndHorizontal();

            GUILayout.Space(yPadding);

            return itemIndex;
        }

        private int RenderEvalScope(MEB_BaseBehaviourData_Item item, int itemIndex)
        {
            int xPadding = 8;
            int yPadding = 8;

            GUILayout.BeginHorizontal();
            GUILayout.Space(xPadding);
      
            GUILayout.BeginVertical(EditorStyles.helpBox);

           item.m_nonColapsed = EditorGUILayout.Foldout(item.m_nonColapsed, "evaluration section");
            if(item.m_nonColapsed == true)
            { 
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                itemIndex = RenderScopeIndexField(itemIndex);
                itemIndex = RenderDeleteField(itemIndex);

                GUILayout.EndHorizontal();

                GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(2));
                GUILayout.Label("evaluators", EditorStyles.boldLabel); //eval
                MEB_BaseBehaviourData_ItemSettings managerData = RenderAddManagerField(false, false, true);

                if (managerData != null)
                {
                    item.m_evalurators.Add(managerData);
                }

                for (int i = 0; i < item.m_evalurators.Count; i++)
                {
                    int moveToIndex = RenderManager(item.m_evalurators[i], i);

                    if (moveToIndex < 0)
                    {
                        item.m_evalurators.RemoveAt(i);
                    }
                    else
                    {
                        if (moveToIndex != i && moveToIndex < item.m_evalurators.Count)
                        {
                            MEB_BaseBehaviourData_ItemSettings subItem = item.m_evalurators[i];
                            item.m_evalurators.RemoveAt(i);
                            item.m_evalurators.Insert(moveToIndex, subItem);
                        }
                    }
                }
                GUILayout.EndVertical(); //section divide

                GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(2));
                GUILayout.Label("managers to evalurate", EditorStyles.boldLabel); //eval data
                managerData = RenderAddManagerField(true, false, false);

                if (managerData != null)
                {
                    item.m_useInEval.Add(managerData);
                }

                for (int i = 0; i < item.m_useInEval.Count; i++)
                {
                    int moveToIndex = RenderManager(item.m_useInEval[i], i);

                    if (moveToIndex < 0)
                    {
                        item.m_useInEval.RemoveAt(i);
                    }
                    else
                    {
                        if (moveToIndex != i && moveToIndex < item.m_useInEval.Count)
                        {
                            MEB_BaseBehaviourData_ItemSettings subItem = item.m_useInEval[i];
                            item.m_useInEval.RemoveAt(i);
                            item.m_useInEval.Insert(moveToIndex, subItem);
                        }
                    }
                }
                GUILayout.EndVertical(); //section divide
            }
            GUILayout.EndVertical();
            
            GUILayout.Space(xPadding);
            GUILayout.EndHorizontal();

            GUILayout.Space(yPadding);

            return itemIndex;
        }

        private int RenderScopeStart(MEB_BaseBehaviourData_ChainScopeItemWapper item, int itemIndex, bool isMainThreadScope)
        {
            int xPadding = 8;
            int yPadding = 8;

            GUILayout.BeginHorizontal();
            GUILayout.Space(xPadding);

            GUILayout.BeginVertical(EditorStyles.helpBox);

           item.m_nonColapsed = EditorGUILayout.Foldout(item.m_nonColapsed, "scope section");
            if(item.m_nonColapsed == true)
            { 
                GUILayout.BeginHorizontal();
                MEB_BaseBehaviourData_ItemSettings managerData = RenderAddManagerField(true, true, false);
                MEB_BaseBehaviourData_Item evalData = RenderAddEvalField();


                GUILayout.FlexibleSpace();

                itemIndex = RenderScopeIndexField(itemIndex);
                itemIndex = RenderDeleteField(itemIndex);

                if (itemIndex == 0)
                {
                    int i = 0;
                }

                GUILayout.EndHorizontal();
                GUILayout.Space(yPadding);

                if (isMainThreadScope == true)
                {
                    GUILayout.Label("main thread scope data:", EditorStyles.boldLabel);
                    GUILayout.Space(yPadding);

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                }
                else 
                {
                    GUILayout.BeginVertical();
                }

                if (managerData != null)
                {
                    item.m_items.Add(new MEB_BaseBehaviourData_Item());
                    item.m_items[item.m_items.Count - 1].m_noneEvalurationManager = managerData;
                }

                if (evalData != null)
                {
                    item.m_items.Add(evalData);
                }

                return itemIndex;
            }
            else
            {
                GUILayout.BeginVertical();
                itemIndex = -2;
            }

            return itemIndex;
        }

        private void RenderScopeEnd()
        {
            int xPadding = 8;
            int yPadding = 8;

            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.Space(xPadding);
            GUILayout.EndHorizontal();

            GUILayout.Space(yPadding);
        }

        private bool RootManagerMovement(int moveLoc, int oldLoc, int i, int j)
        {
            if (moveLoc >= 0 && moveLoc != oldLoc) //handles movment
            {
                if (m_loadedObject.m_items[i].m_isScoped == false) //has to move scope as well
                {
                    if (moveLoc < m_loadedObject.m_items.Count)
                    {
                        MEB_BaseBehaviourData_ChainScopeItemWapper item = m_loadedObject.m_items[i];
                        m_loadedObject.m_items.RemoveAt(i);
                        m_loadedObject.m_items.Insert(moveLoc, item);
                    }
                }
                else
                {
                    if (moveLoc < m_loadedObject.m_items[i].m_items.Count)
                    {
                        MEB_BaseBehaviourData_Item item = m_loadedObject.m_items[i].m_items[j];
                        m_loadedObject.m_items[i].m_items.RemoveAt(j);
                        m_loadedObject.m_items[i].m_items.Insert(moveLoc, item);
                    }
                }
            }

            if (moveLoc < 0)
            {
                m_loadedObject.m_items[i].m_items.RemoveAt(j);

                if (m_loadedObject.m_items[i].m_isScoped == false)
                {
                    m_loadedObject.m_items.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private void Update()
        {
            if (m_loadedObject == null)
            {
                Close();
            }

            if (Application.isPlaying == true)
            {
                m_currentRefreshRatePoint += Time.deltaTime;

                if (m_currentRefreshRatePoint >= m_refreshRateMax)
                {
                    Repaint();
                    m_currentRefreshRatePoint = 0;
                }
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            MEB_BaseBehaviourData_ItemSettings managerData = RenderAddManagerField(true, false, false);
            MEB_BaseBehaviourData_Item evalData = RenderAddEvalField();
            MEB_BaseBehaviourData_ChainScopeItemWapper scopeData = RenderAddScopeField();
            GUILayout.EndHorizontal();


            if (Application.isPlaying == true && m_loadedObject.m_runtimeName != "")
            {
                GUILayout.Label($"debug data is from object: {m_loadedObject.m_runtimeName}");
            }

            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);

            if (managerData != null)
            {
                m_loadedObject.m_items.Add(new MEB_BaseBehaviourData_ChainScopeItemWapper());

                m_loadedObject.m_items[m_loadedObject.m_items.Count - 1].m_items = new List<MEB_BaseBehaviourData_Item>{ new MEB_BaseBehaviourData_Item() };
                m_loadedObject.m_items[m_loadedObject.m_items.Count - 1].m_items[0].m_noneEvalurationManager = managerData;
            }

            if (evalData != null)
            {
                m_loadedObject.m_items.Add(new MEB_BaseBehaviourData_ChainScopeItemWapper());

                m_loadedObject.m_items[m_loadedObject.m_items.Count - 1].m_items = new List<MEB_BaseBehaviourData_Item> { evalData };
            }

            if (scopeData != null)
            {
                m_loadedObject.m_items.Add(scopeData);
            }

            for (int i = 0; i < m_loadedObject.m_items.Count; i++)
            {
                int scopeMovement = i;

                if (m_loadedObject.m_items[i].m_isScoped == true) //this is a scope render the scope wapper
                {
                    scopeMovement = RenderScopeStart(m_loadedObject.m_items[i], i, m_loadedObject.m_items[i].m_isForMainThread);
                }

                if(scopeMovement >= 0)
                {
                    for (int j = 0; j < m_loadedObject.m_items[i].m_items.Count; j++) //go thougth items
                    {
                        if (m_loadedObject.m_items[i].m_items[j].m_isNormalManager == true) //is not evaluration scope
                        {
                            int managerIndexDecomposed = j; //get its index
                            if(m_loadedObject.m_items[i].m_isScoped == false)
                            {
                                managerIndexDecomposed = i;
                            }

                            int managerMovment = RenderManager(m_loadedObject.m_items[i].m_items[j].m_noneEvalurationManager, managerIndexDecomposed); //render manager

                            if (RootManagerMovement(managerMovment, managerIndexDecomposed, i, j) == true)
                            {
                                EditorGUILayout.EndScrollView();
                                return;
                            }
                        }
                        else //is evaluration scope
                        {
                            int evalIndexDecomposed = j;
                            if (m_loadedObject.m_items[i].m_isScoped == false)
                            {
                                evalIndexDecomposed = i;
                            }

                            int evalMovement = RenderEvalScope(m_loadedObject.m_items[i].m_items[j], evalIndexDecomposed);

                            if (RootManagerMovement(evalMovement, evalIndexDecomposed, i, j) == true)
                            {
                                EditorGUILayout.EndScrollView();
                                return;
                            }
                        }
                    }
                }

                if (m_loadedObject.m_items[i].m_isScoped == true)
                {
                    RenderScopeEnd();

                    if (scopeMovement == -1)
                    {
                        m_loadedObject.m_items.RemoveAt(i);
                        EditorGUILayout.EndScrollView();
                        return;
                    }
                    else if (scopeMovement >= 0 && scopeMovement != i && i < m_loadedObject.m_items.Count)
                    {
                        MEB_BaseBehaviourData_ChainScopeItemWapper item = m_loadedObject.m_items[i];
                        m_loadedObject.m_items.RemoveAt(i);
                        m_loadedObject.m_items.Insert(scopeMovement, item);
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