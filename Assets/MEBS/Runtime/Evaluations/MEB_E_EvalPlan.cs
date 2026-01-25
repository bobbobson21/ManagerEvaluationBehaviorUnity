
using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MEBS.Editor
{
    [InitializeOnLoad]
    public class MEB_E_EvalPlan_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalPlan_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalPlan_UI());
        }

        public MEB_E_EvalPlan_UI()
        {
            m_name = "MEB_E_EvalPlan";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = $"Gets all the managers to return an int and Executes them in order of highest retured int to lowest ingoring all managers that return {int.MinValue}. A given manager will not stop being Executed untill it fails self evaluration or untill it callesback MEB_I_BoolScoop.GetBoolEvalValue() with false to say the current plan failed";

            return data;
        }
    }
}
#endif

namespace MEBS.Runtime
{
    public class MEB_E_EvalPlan_DataPoint
    {
        public MEB_BaseManager m_managerToExecute = null;
        public int m_arrayIndex = 0;
        public int m_score = 0;
    }

    public class MEB_E_EvalPlan : MEB_BaseManager, MEB_I_EvalScoop
    {
        private int m_startPointOfScope = 0;
        private int m_endPointOfScope = 0;

        private List<MEB_E_EvalPlan_DataPoint> m_sortedManagers = new List<MEB_E_EvalPlan_DataPoint>();
        int m_currentExecutionIndex = 0;

        public void SetEvaluationScope(int start, int end)
        {
            m_startPointOfScope = start;
            m_endPointOfScope = end;
        }

        public override void EvaluationStart(int index, float delta)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);

            if (m_sortedManagers.Count == 0) //build plan
            {
                for (int i = 0; i < arrayLength; i++)
                {
                    int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                    MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                    MEB_E_EvalPlan_DataPoint dataPoint = new MEB_E_EvalPlan_DataPoint();
                    dataPoint.m_managerToExecute = manager;
                    dataPoint.m_score = int.MinValue;
                    dataPoint.m_arrayIndex = i;

                    try
                    {
                        dataPoint.m_score = ((MEB_I_IntScoop)manager).GetIntEvalValue(delta);
                    }
                    catch
                    {
                        Debug.LogError($"ERROR: MEB_EP_ES_TC1==F: EvalHighest failed to eval item at index ({i}) for unkown reasons");
                    }

                    m_sortedManagers.Add(dataPoint);
                }

                m_sortedManagers.Sort((a,b) => (a.m_score.CompareTo( b.m_score )));
            }

            if (m_currentExecutionIndex < m_sortedManagers.Count && m_sortedManagers[m_currentExecutionIndex].m_score > int.MinValue) //can we Execute this part of the plan
            {
                for (int i = 0; i < arrayLength; i++) //if yes black all but current manager
                {
                    int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                    MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                    if (i != m_sortedManagers[m_currentExecutionIndex].m_arrayIndex)
                    {
                        manager.BlockMoveToExecutionForCycle();
                    }
                }

                int ExecuteManagerIndex = ((index + m_endPointOfScope) - arrayLength) + m_sortedManagers[m_currentExecutionIndex].m_arrayIndex;
                MEB_BaseManager ExecuteManager = m_director.GetManagerByIndex(ExecuteManagerIndex);

                if (ExecuteManager.IsAllowedToExecute() == false) //find out if current manager is done Executing
                {
                    m_currentExecutionIndex++;
                }

                try
                {
                    if(((MEB_I_BoolScoop)ExecuteManager).GetBoolEvalValue(delta) == false) //also check to see if the current manager wants the whole plan redone 
                    {
                        m_currentExecutionIndex = 0;
                        m_sortedManagers.Clear();
                    }
                }
                catch
                {
                }
            }
            else //if we can not Execute this part redo plan
            {
                m_currentExecutionIndex = 0;
                m_sortedManagers.Clear();
            }
        }

        public override void EvaluationEnd(int index, float delta)
        {
            BlockMoveToExecutionForCycle();
        }
    }
}
