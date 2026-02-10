using MEBS.Editor;
using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MEBS.Editor
{
    [InitializeOnLoad]
    public class MEB_E_EvalManagerCondBlock_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalManagerCondBlock_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalManagerCondBlock_UI());
        }

        public MEB_E_EvalManagerCondBlock_UI()
        {
            m_name = "MEB_E_EvalManagerCondBlock";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_ManagerCondBlockSettings data = new MEB_BaseBehaviourData_ManagerCondBlockSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = "if any of the manages betweeen the blocked section are blocked then all of them are blocked. If any mamager in the blocked section is blocked the managers escape section will be allowed to exacute otherwise they will not be.";

            return data;
        }
    }
}
#endif

namespace MEBS.Runtime
{
    public class MEB_BaseBehaviourData_ManagerCondBlockSettings : MEB_BaseBehaviourData_ItemSettings
    {
        public bool m_inverted = false;

        public int m_blockRangeStartPoint = 0;
        public int m_blockRangeEndPoint = int.MaxValue;

        public int m_escapeRangeStartPoint = int.MaxValue;
        public int m_escapeRangeEndPoint = int.MaxValue;

        private bool m_p_isRenderingScope = false;

#if UNITY_EDITOR
        public override void OnGUI()
        {
            int spaceY = 8;

            GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
            m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");


            if (m_displayCustomSettingExpanded == true)
            {
                if (MEB_UI_BehaviourEditor.InRestrictedEditMode() == false)
                {
                    m_inverted = EditorGUILayout.Toggle("invert logic", m_inverted);
                    GUILayout.Space(spaceY);

                    int.TryParse(EditorGUILayout.TextField("block range start", m_blockRangeStartPoint.ToString()), out m_blockRangeStartPoint);
                    int.TryParse(EditorGUILayout.TextField("block range end", m_blockRangeEndPoint.ToString()), out m_blockRangeEndPoint);

                    int.TryParse(EditorGUILayout.TextField("escape range start", m_escapeRangeStartPoint.ToString()), out m_escapeRangeStartPoint);
                    int.TryParse(EditorGUILayout.TextField("escape range end", m_escapeRangeEndPoint.ToString()), out m_escapeRangeEndPoint);

                    if (m_escapeRangeStartPoint <= m_blockRangeEndPoint) { m_escapeRangeStartPoint = m_blockRangeEndPoint + 1; }
                    if (m_escapeRangeEndPoint <= m_blockRangeEndPoint) { m_escapeRangeEndPoint = m_blockRangeEndPoint + 1; }
                }
                else
                {
                    EditorGUILayout.Toggle("invert logic", m_inverted);
                    GUILayout.Space(spaceY);

                    MEB_GUI_Layout.LockedInputStyle("block range start", m_blockRangeStartPoint.ToString());
                    MEB_GUI_Layout.LockedInputStyle("block range end", m_blockRangeEndPoint.ToString());

                    MEB_GUI_Layout.LockedInputStyle("block range start", m_escapeRangeStartPoint.ToString());
                    MEB_GUI_Layout.LockedInputStyle("block range end", m_escapeRangeEndPoint.ToString());
                }
            }

            GUILayout.EndVertical();
        }

        public override void OnManagerInEvalurationScopeStartGUI(int relativeScopeIndex)
        {
            if (relativeScopeIndex == m_blockRangeStartPoint && m_p_isRenderingScope == false)
            {
                m_p_isRenderingScope = true;
                Color col = Color.magenta;

                MEB_GUI_Layout.BeginAffectBox(col);
            }

            if (relativeScopeIndex == m_escapeRangeStartPoint && m_p_isRenderingScope == false)
            {
                m_p_isRenderingScope = true;
                Color col = Color.lavender;

                MEB_GUI_Layout.BeginAffectBox(col);
            }
        }

        public override void OnManagerInEvalurationScopeEndGUI(int relativeScopeIndex)
        {
            if ((relativeScopeIndex == m_blockRangeEndPoint || relativeScopeIndex == m_escapeRangeEndPoint || relativeScopeIndex == int.MaxValue) && m_p_isRenderingScope == true)
            {
                m_p_isRenderingScope = false;
                MEB_GUI_Layout.EndAffectBox();
            }
        }
#endif
    }

    public class MEB_E_EvalManagerCondBlock : MEB_BaseManager, MEB_I_EvalScoop
    {
        private bool m_inverted = false;
        public int m_blockRangeStartPoint = 0;
        public int m_blockRangeEndPoint = int.MaxValue;

        public int m_escapeRangeStartPoint = int.MaxValue;
        public int m_escapeRangeEndPoint = int.MaxValue;

        private int m_startPointOfScope = 0;
        private int m_endPointOfScope = 0;


        public void SetEvaluationScope(int start, int end)
        {
            m_startPointOfScope = start;
            m_endPointOfScope = end;
        }

        public override void OnInitialized()
        {
            MEB_BaseBehaviourData_ManagerCondBlockSettings settings = (MEB_BaseBehaviourData_ManagerCondBlockSettings)m_itemSettings;

            if (settings != null)
            {
                m_inverted = settings.m_inverted;
                m_blockRangeStartPoint = settings.m_blockRangeStartPoint;
                m_blockRangeEndPoint = settings.m_blockRangeEndPoint;
                m_escapeRangeStartPoint = settings.m_escapeRangeStartPoint;
                m_escapeRangeEndPoint = settings.m_escapeRangeEndPoint;
            }
        }

        public override void EvaluationStart(int index, float delta)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);
            bool isblocked = m_inverted;

            for (int i = 0; i < arrayLength; i++)
            {
                int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (manager.IsAllowedToExecute() == false && i < m_escapeRangeStartPoint == true) //fins out if we are blocked or not
                {
                    isblocked = !m_inverted;
                }
            }

            for (int i = 0; i < arrayLength; i++)
            {
                int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (isblocked == true)
                {
                    if (i >= m_blockRangeStartPoint && i <= m_blockRangeEndPoint) //blocks the block range
                    {
                        manager.BlockMoveToExecutionForCycle();
                    }
                }
                else
                {
                    if (i >= m_escapeRangeStartPoint && i <= m_escapeRangeStartPoint) //blocks the start range
                    {
                        manager.BlockMoveToExecutionForCycle();
                    }
                }
            }
        }

        public override void EvaluationEnd(int index, float delta)
        {
            BlockMoveToExecutionForCycle();
        }
    }
}
