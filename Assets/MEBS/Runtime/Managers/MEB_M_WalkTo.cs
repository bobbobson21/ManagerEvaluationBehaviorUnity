using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace MEBS.Editor
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class MEB_M_WalkTo_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_M_WalkTo_UI()
        {
            MEB_UI_BehaviourEditor.AddNormalManager(new MEB_M_WalkTo_UI());
        }

        public MEB_M_WalkTo_UI()
        {
            m_name = "MEB_M_WalkTo";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = "Makes the NPC move towards a desired location. \n\nvalid blackboard data: \ntarget: (vector3BlackboardKeyAsString) \nspeed: (floatBlackboardKeyAsString) def = (navAgentSpeed)";

            return data;
        }
    }
#endif
}

namespace MEBS.Runtime
{
    public class MEB_M_WalkTo : MEB_BaseManager
    {
        private string m_speedKey = "";
        private string m_targetKey = "";
        private NavMeshAgent m_agent = null;

        private Vector3 m_lastLocation = Vector3.zero;

        public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
        {
            for (int i = 0; i < idenifyers.Count; i++)
            {
                if (idenifyers[i] == "target")
                {
                    m_targetKey = keys[i];
                }

                if (idenifyers[i] == "speed")
                {
                    m_speedKey = keys[i];
                }
            }
        }

        public override void EvaluationEnd(int index)
        {
            if (((Vector3)m_director.m_blackboard.GetObject(m_targetKey)) == Vector3.zero)
            {
                BlockMoveToExecutionForCycle();
            }
        }

        public override void OnInitialized()
        {
            m_agent = m_director.m_gameObject.GetComponent<NavMeshAgent>();
        }

        public override void OnUpdate(float delta, int index)
        {
            Vector3 currentLocation = (Vector3)m_director.m_blackboard.GetObject(m_targetKey);

            if ((m_lastLocation - currentLocation).magnitude <= 0.5f)
            {
                m_agent.SetDestination(currentLocation);

                if(m_agent.isOnNavMesh == false)
                {
                    Debug.Log($"{m_director.m_gameObject.transform.parent.gameObject}");
                }
            }

            m_lastLocation = currentLocation;

            if (m_speedKey != "")
            {
                m_agent.speed = ((float)m_director.m_blackboard.GetObject(m_speedKey));
            }
        }

    }
}
