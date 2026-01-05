using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR 
namespace MEBS.Editor
{
    [InitializeOnLoad]
    public class MEB_E_EvalRandom_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalRandom_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalRandom_UI());
        }

        public MEB_E_EvalRandom_UI()
        {
            m_name = "MEB_E_EvalRandom";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = "Pickes a manager from a list of not blocked managers and bloacks all the ones that isnt the one picked.";

            return data;
        }
    }
}
#endif

namespace MEBS.Runtime
{
    public class MEB_E_EvalRandom : MEB_BaseManager, MEB_I_EvalScoop
    {
        private int m_startPointOfScope = 0;
        private int m_endPointOfScope = 0;

        public void SetEvaluationScope(int start, int end)
        {
            m_startPointOfScope = start;
            m_endPointOfScope = end;
        }

        public override void EvaluationStart(int index, float delta)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);

            int indexOfPickedItem = -1;
            List<MEB_BaseManager> managers = new List<MEB_BaseManager>();

            for (int i = 0; i < arrayLength; i++) //find highest value
            {
                int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (manager.IsAllowedToExecute() == true)
                { 
                    managers.Add(manager);
                }
            }

            indexOfPickedItem = Random.Range(0, managers.Count -1);

            for (int i = 0; i < managers.Count; i++)
            {
                if (i != indexOfPickedItem)
                {
                    managers[i].BlockMoveToExecutionForCycle();
                }
            }
        }

        public override void EvaluationEnd(int index, float delta)
        {
            BlockMoveToExecutionForCycle();
        }
    }
}