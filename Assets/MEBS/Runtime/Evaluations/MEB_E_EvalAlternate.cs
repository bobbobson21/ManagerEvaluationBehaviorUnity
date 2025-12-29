
using MEBS.Runtime;
using UnityEditor;

namespace MEBS.Editor
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class MEB_E_EvalAlternate_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalAlternate_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalAlternate_UI());
        }

        public MEB_E_EvalAlternate_UI()
        {
            m_name = "MEB_E_EvalAlternate";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = "Sequentially alternates between all the manager in the 'manager to evalurate' section to only exacute one of them. Managers which failed to make it to exacuteion before this will be skiped over.";

            return data;
        }
    }
#endif
}

namespace MEBS.Runtime
{
    public class MEB_E_EvalAlternate : MEB_BaseManager, MEB_I_EvalScoop
    {
        private int m_startPointOfScope = 0;
        private int m_endPointOfScope = 0;
        private int m_currentPoint = 0;

        public void SetEvaluationScope(int start, int end)
        {
            m_startPointOfScope = start;
            m_endPointOfScope = end;
        }

        public override void EvaluationStart(int index)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);

            for (int i = 0; i < arrayLength; i++)
            {
                int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (i != m_currentPoint)
                {
                    manager.BlockMoveToExecutionForCycle();
                }
            }

            m_currentPoint++;

            if (m_currentPoint >= arrayLength)
            {
                m_currentPoint = 0;
            }
        }

        public override void EvaluationEnd(int index)
        {
            BlockMoveToExecutionForCycle();
        }
    }
}
