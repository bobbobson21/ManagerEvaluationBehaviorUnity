using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_EvadeAvoidanceAreas_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_EvadeAvoidanceAreas_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_EvadeAvoidanceAreas_UI());
    }

    public UserManger_EvadeAvoidanceAreas_UI()
    {
        m_name = "UserManger_EvadeAvoidanceAreas";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_EvadeAvoidanceAreas";
        data.m_displayName = m_name;
        data.m_displayDiscription = "keeps us out of areas of the map that we should avoid" +
            "\n\nvaild blackboard data: " +
            "\nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";
        return data;
    }
}
#endif

public class UserManger_EvadeAvoidanceAreas : MEB_BaseManager, MEB_I_IntScoop
{
    private AICAvoidanceAreas m_currentArea = null;
    private NavMeshAgent m_agent = null;
    private string m_storeTargetLocationInKey = "";

    private float m_holdTime = 0;
    private float m_holdTimeMax = 4;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index, float delta)
    {
        m_currentArea = null;
        m_holdTime -= Time.deltaTime;

        if(m_holdTime > 0)
        {
            BlockMoveToExecutionForCycle();
            return;
        }

        for (int i = 0; i < AICAvoidanceAreas.m_totalAvoidanceAreas.Count; i++)
        {
            Bounds testBox = new Bounds();
            testBox.center = AICAvoidanceAreas.m_totalAvoidanceAreas[i].transform.position; 
            testBox.size = AICAvoidanceAreas.m_totalAvoidanceAreas[i].m_avoidArea;

            if(testBox.Contains(m_director.m_gameObject.transform.position) == true)
            {
                m_currentArea = AICAvoidanceAreas.m_totalAvoidanceAreas[i];
                break;
            }
        }

        if(m_currentArea == null)
        {
            BlockMoveToExecutionForCycle();
        }
    }

    public override void OnInitialized()
    {
        m_agent = m_director.m_gameObject.GetComponent<NavMeshAgent>();
    }

    public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters Executeion
    {
    }

    public override void OnEnd()
    {
        m_currentArea = null;
    }

    public override void OnUpdate(float delta, int index)
    {
        if(m_agent.velocity.magnitude <= 0.5f)
        {
            m_holdTime = m_holdTimeMax;
        }

        Vector3 destanation = m_director.m_gameObject.transform.position - (m_currentArea.transform.position + m_currentArea.m_avoidPoint);
        destanation.z = 0;

        destanation = destanation.normalized *6;
        destanation += m_director.m_gameObject.transform.position;

        m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, destanation);
    }

    public int GetIntEvalValue(float delta)
    {
        return 3;
    }
}
