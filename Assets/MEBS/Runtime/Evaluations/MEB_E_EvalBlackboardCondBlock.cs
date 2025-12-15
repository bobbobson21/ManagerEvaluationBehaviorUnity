using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MEBS.Editor
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class MEB_E_EvalBlackboardCondBlock_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalBlackboardCondBlock_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalBlackboardCondBlock_UI());
        }

        public MEB_E_EvalBlackboardCondBlock_UI()
        {
            m_name = "MEB_E_EvalBlackboardCondBlock";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_BlackboardCondBlockSettings data = new MEB_BaseBehaviourData_BlackboardCondBlockSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = "If any bool registed to this managers black board list returns false all the managers int 'managers to evalurate' section are blocked from being exacuted not just the first one. \n\nvalid blackboard data: \n???: (BoolBlackboardKeyAsString)";

            return data;
        }
    }
#endif
}

namespace MEBS.Runtime
{
    public class MEB_BaseBehaviourData_BlackboardCondBlockSettings : MEB_BaseBehaviourData_ItemSettings
    {
        public bool m_inverted = false;

        public int m_blockRangeStartPoint = 0;
        public int m_blockRangeEndPoint = int.MaxValue;

        public override void OnGUI()
        {
            int spaceY = 8;

            GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
            m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

            if (m_displayCustomSettingExpanded == true)
            {
                m_inverted = EditorGUILayout.Toggle("invert logic", m_inverted);
                GUILayout.Space(spaceY);

                int.TryParse(EditorGUILayout.TextField("block range start", m_blockRangeStartPoint.ToString()), out m_blockRangeStartPoint);
                int.TryParse(EditorGUILayout.TextField("block range end", m_blockRangeEndPoint.ToString()), out m_blockRangeEndPoint);
            }

            GUILayout.EndVertical();
        }
    }

    public class MEB_E_EvalBlackboardCondBlock : MEB_BaseManager, MEB_I_EvalScope
    {
        private bool m_inverted = false;
        private int m_blockRangeStartPoint = 0;
        private int m_blockRangeEndPoint = int.MaxValue;

        private int m_startPointOfScope = 0;
        private int m_endPointOfScope = 0;
        private List<string> m_boolsToEval = new List<string>();

        public void SetEvaluationScope(int start, int end)
        {
            m_startPointOfScope = start;
            m_endPointOfScope = end;
        }

        public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
        {
            m_boolsToEval = keys;
        }

        public override void OnInitialized()
        {
            MEB_BaseBehaviourData_BlackboardCondBlockSettings settings = (MEB_BaseBehaviourData_BlackboardCondBlockSettings)m_itemSettings;

            if (settings != null)
            {
                m_inverted = settings.m_inverted;
                m_blockRangeStartPoint = settings.m_blockRangeStartPoint;
                m_blockRangeEndPoint = settings.m_blockRangeEndPoint;
            }
        }

        public override void EvaluationStart(int index)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);
            bool conditionOfEval = !m_inverted;

            for (int i = 0; i < m_boolsToEval.Count; i++) //if a bool is false we enter fail
            {
                try
                {
                    if (((bool)m_director.m_blackboard.GetObject(m_boolsToEval[i])) == false)
                    {
                        conditionOfEval = m_inverted;
                        break;
                    }
                }
                catch
                {
                    Debug.LogError($"ERROR: MEB_EBCB_ES_TC==F: EvalBlackboardCondBlock failed to obtain value from blackboard with key ({m_boolsToEval[i]}, {i}) for unkown reasons");
                }
            }

            for (int i = 0; i < arrayLength; i++)
            {
                int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (conditionOfEval == false && i >= m_blockRangeStartPoint && i <= m_blockRangeEndPoint)
                {
                    manager.BlockMoveToExecutionForCycle();
                }
            }

        }

        public override void EvaluationEnd(int index)
        {
            BlockMoveToExecutionForCycle();
        }
    }
}
