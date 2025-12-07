using MEBS.Runtime;
using UnityEditor;
using UnityEngine;

namespace MEBS.Editor
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class MEB_E_EvalHighest_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalHighest_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalHighest_UI());
        }

        public MEB_E_EvalHighest_UI()
        {
            m_name = "MEB_E_EvalHighest";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = "Gets all the managers in the 'managers to evalurate' section to return an int thougth MEB_I_IntScoop.GetIntEvalValue(). The one that returns the higest value, that also isn't already blocked, moves on the rest are blocked from execution.";

            return data;
        }
    }
}
#endif

namespace MEBS.Runtime
{
    public class MEB_E_EvalHighest : MEB_BaseManager, MEB_I_EvalScope
    {
        private int m_startPointOfScope = 0;
        private int m_endPointOfScope = 0;

        public void SetEvaluationScope(int start, int end)
        {
            m_startPointOfScope = start;
            m_endPointOfScope = end;
        }

        public override void EvaluationStart(int index)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);

            int indexOfHighestRatedManagerSoFar = -1;
            int highestValueSoFar = int.MinValue;

            for (int i = 0; i < arrayLength; i++) //find highest value
            {
                try
                {
                    int otherManagerIndex = (index - arrayLength) +i;
                    MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);
                    int testValue = ((MEB_I_IntScoop)manager).GetIntEvalValue();

                    if (testValue > highestValueSoFar && manager.IsAllowedToExecute() == true)
                    {
                        highestValueSoFar = testValue;
                        indexOfHighestRatedManagerSoFar = i;
                    }
                }
                catch
                {
                    Debug.LogError($"ERROR: MEB_EH_ES_TC==F: EvalHighest failed to eval item at index ({i}) for unkown reasons");
                }
            }

            for (int i = 0; i < arrayLength; i++) //blocks all but highest evalurated item from entering exacuteion
            {
                int otherManagerIndex = (index - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (indexOfHighestRatedManagerSoFar != i)
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