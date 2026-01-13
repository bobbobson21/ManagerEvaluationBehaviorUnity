using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_StickWithOtherTeammates_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_StickWithOtherTeammates_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_StickWithOtherTeammates_UI());
    }

    public UserManger_StickWithOtherTeammates_UI()
    {
        m_name = "UserManger_StickWithOtherTeammates";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_StickWithOtherTeammates";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the AI follow its team leader.\n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_StickWithOtherTeammates : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_storeTargetLocationInKey = "";

    private AICTeamOparator m_teamOparator = null;
    private UserBlackboard_BasicBadguy m_ourBlackboard = null;

    private bool m_flip = false;
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
        for (int i = 0; i < m_teamOparator.GetAllOnMyTeam().Count; i++)
        {
            UserBlackboard_BasicBadguy teamMate = (UserBlackboard_BasicBadguy)m_teamOparator.GetBlackboardOfTeamMate(i);

            if (teamMate.m_resourceCount < teamMate.m_desiredResourceCount && m_director.m_gameObject.tag == teamMate.gameObject.tag)
            {
                return;
            }
        }

        BlockMoveToExecutionForCycle();
    }

    public override void OnInitialized()
    {
        m_teamOparator = m_director.m_gameObject.GetComponent<AICTeamOparator>();
        m_ourBlackboard = (UserBlackboard_BasicBadguy)m_director.m_blackboard;

        m_flip = (Random.Range(0, 1) == 1);
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        int teamCount = m_teamOparator.GetAllOnMyTeam().Count;

        for (int i = 0; i < teamCount; i++)
        {
            int loopIndex = i;

            if (m_flip == true)
            {
                loopIndex = (teamCount - 1) - loopIndex;
            }

            UserBlackboard_BasicBadguy teamMate = (UserBlackboard_BasicBadguy)m_teamOparator.GetBlackboardOfTeamMate(loopIndex);

            if (teamMate.m_resourceCount < teamMate.m_desiredResourceCount && m_director.m_gameObject.tag == teamMate.gameObject.tag)
            {
                if ((teamMate.gameObject.transform.position - m_director.m_gameObject.transform.position).magnitude > 3)
                {
                    m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, teamMate.gameObject.transform.position);
                }
                else
                {
                    m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, teamMate.gameObject.transform.position + (new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f))).normalized * 3);
                }  
            }
        }
    }

    public int GetIntEvalValue(float delta)
    {
        if (m_ourBlackboard.m_resourceCount >= m_ourBlackboard.m_desiredResourceCount)
        {
            return 50;
        }

        return 0;
    }
}
