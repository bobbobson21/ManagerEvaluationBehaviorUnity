using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MEBS.Editor
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class MEB_E_EvalBlackboardCond_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalBlackboardCond_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalBlackboardCond_UI());
        }

        public MEB_E_EvalBlackboardCond_UI()
        {
            m_name = "MEB_E_EvalBlackboardCond";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = "If every bool registed to this managers black board list returns true the all managers in 'managers to evalurate' section after the first one are blocked, if one of the bools equals false the first manager is blocked. \n\nvalid blackboard data: \n???: (BoolBlackboardKeyAsString)";

            return data;
        }
    }
#endif
}

namespace MEBS.Runtime
{
    public class MEB_E_EvalBlackboardCond : MEB_BaseManager, MEB_I_EvalScope
    {
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

        public override void EvaluationStart(int index)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);
            bool conditionOfEval = true;

            for (int i = 0; i < m_boolsToEval.Count; i++) //if a bool is false we enter fail
            {
                try
                {
                    if (((bool)m_director.m_blackboard.GetObject(m_boolsToEval[i])) == false)
                    {
                        conditionOfEval = false;
                        break;
                    }
                }
                catch
                {
                    Debug.LogError($"ERROR: MEB_EBC_ES_TC==F: EvalBlackboardCond failed to obtain value from blackboard with key ({m_boolsToEval[i]}, {i}) for unkown reasons");
                }
            }

            for (int i = 0; i < arrayLength; i++)
            {
                int otherManagerIndex = (index - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (conditionOfEval == false)
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
