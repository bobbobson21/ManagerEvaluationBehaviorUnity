using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
namespace MEBS.Editor
{
    [InitializeOnLoad]
    public class MEB_E_EvalFirstUnblocked_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalFirstUnblocked_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalFirstUnblocked_UI());
        }

        public MEB_E_EvalFirstUnblocked_UI()
        {
            m_name = "MEB_E_EvalFirstUnblocked";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
            data.m_class = "MEB_E_EvalFirstUnblocked";
            data.m_displayName = m_name;
            data.m_displayDiscription = "Blocks all managers from moving down to execution apart from the first unblocked one in the scope of managers to evalurate.";

            return data;
        }
    }
}
#endif

namespace MEBS.Runtime
{
    public class MEB_E_EvalFirstUnblocked : MEB_BaseManager, MEB_I_EvalScoop
    {
        private int m_startPointOfScope = 0;
        private int m_endPointOfScope = 0;

        public void SetEvaluationScope(int start, int end)
        {
            m_startPointOfScope = start;
            m_endPointOfScope = end;
        }

        public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
        {

        }

        public override void EvaluationStart(int index, float delta)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);
            bool foundResult = false;

            for (int i = 0; i < arrayLength; i++)
            {
                int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (foundResult == true)
                {
                    manager.BlockMoveToExecutionForCycle();
                }
                else if (manager.IsAllowedToExecute() == true)
                {
                    foundResult = true;
                }
            }
        }

        public override void EvaluationEnd(int index, float delta)
        {
            BlockMoveToExecutionForCycle();
        }
    }
}
